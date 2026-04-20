using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.AI.Common;
using StudyQuest.API.Models;

namespace StudyQuest.API.Features.AI.Summarize;

public record SummarizeCommand(Guid StudentId, Guid TopicId, string? Content, int? Grade)
    : IRequest<ErrorOr<SummarizeResponse>>;

internal sealed class SummarizeCommandHandler : IRequestHandler<SummarizeCommand, ErrorOr<SummarizeResponse>>
{
    private readonly AppDbContext _db;
    private readonly OpenAIClient _ai;
    private readonly ILogger<SummarizeCommandHandler> _logger;

    public SummarizeCommandHandler(AppDbContext db, OpenAIClient ai, ILogger<SummarizeCommandHandler> logger)
    {
        _db = db;
        _ai = ai;
        _logger = logger;
    }

    public async Task<ErrorOr<SummarizeResponse>> Handle(SummarizeCommand request, CancellationToken ct)
    {
        var student = await _db.Students.FindAsync([request.StudentId], ct);
        if (student is null) return AIErrors.StudentNotFound;

        var grade = request.Grade ?? student.Grade;
        var topic = await _db.Topics.Include(t => t.Notes).FirstOrDefaultAsync(t => t.Id == request.TopicId, ct);
        if (topic is null) return AIErrors.TopicNotFound;

        var content = request.Content ?? WASSCEPromptContext.BuildNoteContent(topic.Notes);
        if (string.IsNullOrWhiteSpace(content)) return AIErrors.NoContent;

        // L1: memory cache
        var cacheKey = $"ai_summary_{request.TopicId}_{grade}_{content.GetHashCode()}";
        var memoryCached = _ai.TryGetCached<SummarizeResponse>(cacheKey);
        if (memoryCached is not null) return memoryCached;

        // L2: DB cache
        var inputHash = ComputeHash($"{request.TopicId}{grade}{content.GetHashCode()}");
        var dbCached = await _db.CachedAIContents.FirstOrDefaultAsync(
            c => c.ContentType == AIContentType.Summary && c.TopicId == request.TopicId && c.InputHash == inputHash, ct);
        if (dbCached is not null)
        {
            var dbResult = JsonSerializer.Deserialize<SummarizeResponse>(dbCached.ResponseJson, OpenAIClient.JsonOptions);
            if (dbResult is not null)
            {
                _ai.SetCache(cacheKey, dbResult, TimeSpan.FromHours(24));
                return dbResult;
            }
        }

        var systemPrompt = $$"""
            {{WASSCEPromptContext.BaseContext}}
            Summarize the following study material for a Grade {{grade}} student into clear, concise bullet points.
            Focus on WASSCE syllabus objectives for this topic. Highlight key points that are commonly tested in WASSCE examinations.
            {{WASSCEPromptContext.ExamFormatGuidance}}
            Return your response as JSON with this exact format:
            { "summary": "A brief 2-3 sentence overview", "keyPoints": ["Point 1", "Point 2", ...] }
            Only output valid JSON, nothing else.
            """;

        try
        {
            var response = await _ai.CallAsync(systemPrompt, $"Topic: {topic.Name}\n\nContent:\n{content}", _ai.Model, temperature: 0.3f);
            var result = JsonSerializer.Deserialize<SummarizeResponse>(response, OpenAIClient.JsonOptions)
                ?? new SummarizeResponse("Summary not available", []);

            _ai.SetCache(cacheKey, result, TimeSpan.FromHours(24));

            // Persist to DB cache
            var entry = dbCached ?? new CachedAIContent
            {
                Id = Guid.NewGuid(),
                ContentType = AIContentType.Summary,
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
            _logger.LogError(ex, "AI summarize failed");
            return AIErrors.ServiceUnavailable;
        }
    }

    private static string ComputeHash(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexStringLower(bytes);
    }
}
