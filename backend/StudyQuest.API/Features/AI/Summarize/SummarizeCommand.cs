using System.Text.Json;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.AI.Common;

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

        var content = request.Content ?? string.Join("\n\n", topic.Notes.Select(n => $"## {n.Title}\n{n.Content}"));
        if (string.IsNullOrWhiteSpace(content)) return AIErrors.NoContent;

        var cacheKey = $"ai_summary_{request.TopicId}_{grade}_{content.GetHashCode()}";
        var cached = _ai.TryGetCached<SummarizeResponse>(cacheKey);
        if (cached is not null) return cached;

        var systemPrompt = $$"""
            You are an educational assistant for South African high school students.
            Summarize the following study material for a Grade {{grade}} student into clear, concise bullet points.
            Return your response as JSON with this exact format:
            { "summary": "A brief 2-3 sentence overview", "keyPoints": ["Point 1", "Point 2", ...] }
            Only output valid JSON, nothing else.
            """;

        try
        {
            var response = await _ai.CallAsync(systemPrompt, $"Topic: {topic.Name}\n\nContent:\n{content}", _ai.Model);
            var result = JsonSerializer.Deserialize<SummarizeResponse>(response, OpenAIClient.JsonOptions)
                ?? new SummarizeResponse("Summary not available", []);

            _ai.SetCache(cacheKey, result, TimeSpan.FromHours(24));
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI summarize failed");
            return AIErrors.ServiceUnavailable;
        }
    }
}
