using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.AI.Common;
using StudyQuest.API.Models;

namespace StudyQuest.API.Features.AI.Explain;

public record ExplainCommand(Guid StudentId, Guid TopicId, string? SpecificQuestion, int? Grade)
    : IRequest<ErrorOr<ExplainResponse>>;

internal sealed class ExplainCommandHandler : IRequestHandler<ExplainCommand, ErrorOr<ExplainResponse>>
{
    private readonly AppDbContext _db;
    private readonly OpenAIClient _ai;
    private readonly ILogger<ExplainCommandHandler> _logger;

    public ExplainCommandHandler(AppDbContext db, OpenAIClient ai, ILogger<ExplainCommandHandler> logger)
    {
        _db = db;
        _ai = ai;
        _logger = logger;
    }

    public async Task<ErrorOr<ExplainResponse>> Handle(ExplainCommand request, CancellationToken ct)
    {
        var student = await _db.Students.FindAsync([request.StudentId], ct);
        if (student is null) return AIErrors.StudentNotFound;

        var grade = request.Grade ?? student.Grade;
        var topic = await _db.Topics.Include(t => t.Subject).FirstOrDefaultAsync(t => t.Id == request.TopicId, ct);
        if (topic is null) return AIErrors.TopicNotFound;

        // L1: memory cache
        var cacheKey = $"ai_explain_{request.TopicId}_{grade}_{request.SpecificQuestion?.GetHashCode() ?? 0}";
        var memoryCached = _ai.TryGetCached<ExplainResponse>(cacheKey);
        if (memoryCached is not null) return memoryCached;

        // L2: DB cache
        var inputHash = ComputeHash($"{request.TopicId}{grade}{request.SpecificQuestion?.GetHashCode() ?? 0}");
        var dbCached = await _db.CachedAIContents.FirstOrDefaultAsync(
            c => c.ContentType == AIContentType.Explanation && c.TopicId == request.TopicId && c.InputHash == inputHash, ct);
        if (dbCached is not null)
        {
            var dbResult = JsonSerializer.Deserialize<ExplainResponse>(dbCached.ResponseJson, OpenAIClient.JsonOptions);
            if (dbResult is not null)
            {
                _ai.SetCache(cacheKey, dbResult, TimeSpan.FromHours(24));
                return dbResult;
            }
        }

        var systemPrompt = $$"""
            {{WASSCEPromptContext.BaseContext}}
            You are a patient, encouraging teacher explaining {{topic.Subject.Name}} concepts to a Grade {{grade}} student in The Gambia.
            Explain concepts strictly within the scope of the WASSCE syllabus.
            Use simple language, real-world examples relevant to West African students, and analogies they can relate to.
            Reference how this topic is typically examined in WASSCE, common pitfalls, and practical exam tips.
            {{WASSCEPromptContext.ExamTipsGuidance}}
            Return your response as JSON:
            { "explanation": "...", "examples": ["..."], "keyTakeaways": ["..."] }
            Only output valid JSON, nothing else.
            """;

        var userContent = string.IsNullOrWhiteSpace(request.SpecificQuestion)
            ? $"Please explain this topic: {topic.Name}\nDescription: {topic.Description}"
            : $"Topic: {topic.Name}\nStudent's question: {request.SpecificQuestion}";

        try
        {
            var response = await _ai.CallAsync(systemPrompt, userContent, _ai.ExplanationModel, temperature: 0.5f);
            var result = JsonSerializer.Deserialize<ExplainResponse>(response, OpenAIClient.JsonOptions)
                ?? new ExplainResponse("Explanation not available", [], []);

            _ai.SetCache(cacheKey, result, TimeSpan.FromHours(24));

            // Persist to DB cache
            var entry = dbCached ?? new CachedAIContent
            {
                Id = Guid.NewGuid(),
                ContentType = AIContentType.Explanation,
                TopicId = request.TopicId,
                InputHash = inputHash,
                StudentGrade = grade
            };
            entry.ResponseJson = JsonSerializer.Serialize(result, OpenAIClient.JsonOptions);
            entry.GeneratedAt = DateTime.UtcNow;
            if (dbCached is null) _db.CachedAIContents.Add(entry);
            await _db.SaveChangesAsync(ct);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI explain failed");
            return AIErrors.ServiceUnavailable;
        }
    }

    private static string ComputeHash(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexStringLower(bytes);
    }
}
