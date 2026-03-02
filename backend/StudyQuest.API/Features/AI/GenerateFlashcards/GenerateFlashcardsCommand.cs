using System.Text.Json;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.AI.Common;
using StudyQuest.API.Models;

namespace StudyQuest.API.Features.AI.GenerateFlashcards;

public record GenerateFlashcardsCommand(Guid StudentId, Guid TopicId, string? Content, int Count)
    : IRequest<ErrorOr<FlashcardResponse>>;

internal sealed class GenerateFlashcardsCommandHandler : IRequestHandler<GenerateFlashcardsCommand, ErrorOr<FlashcardResponse>>
{
    private readonly AppDbContext _db;
    private readonly OpenAIClient _ai;
    private readonly ILogger<GenerateFlashcardsCommandHandler> _logger;

    public GenerateFlashcardsCommandHandler(AppDbContext db, OpenAIClient ai, ILogger<GenerateFlashcardsCommandHandler> logger)
    {
        _db = db;
        _ai = ai;
        _logger = logger;
    }

    public async Task<ErrorOr<FlashcardResponse>> Handle(GenerateFlashcardsCommand request, CancellationToken ct)
    {
        var student = await _db.Students.FindAsync([request.StudentId], ct);
        if (student is null) return AIErrors.StudentNotFound;

        var topic = await _db.Topics.Include(t => t.Notes).FirstOrDefaultAsync(t => t.Id == request.TopicId, ct);
        if (topic is null) return AIErrors.TopicNotFound;

        var subject = await _db.Subjects.FindAsync([topic.SubjectId], ct);
        var content = request.Content ?? string.Join("\n\n", topic.Notes.Select(n => $"## {n.Title}\n{n.Content}"));

        var cacheKey = $"ai_flashcards_{request.TopicId}_{request.Count}_{student.Grade}";
        var cached = _ai.TryGetCached<FlashcardResponse>(cacheKey);
        if (cached is not null) return cached;

        var systemPrompt = $$"""
            You are an educational assistant for South African Grade {{student.Grade}} students studying {{subject?.Name ?? "this subject"}}.
            Generate exactly {{request.Count}} flashcards from the given content.
            Return your response as JSON: { "flashcards": [{ "front": "Question", "back": "Answer" }, ...] }
            Only output valid JSON, nothing else.
            """;

        var userContent = string.IsNullOrWhiteSpace(content)
            ? $"Generate flashcards about the topic: {topic.Name}"
            : $"Topic: {topic.Name}\n\nContent:\n{content}";

        try
        {
            var response = await _ai.CallAsync(systemPrompt, userContent, _ai.Model);
            var result = JsonSerializer.Deserialize<FlashcardResponse>(response, OpenAIClient.JsonOptions)
                ?? new FlashcardResponse([]);

            foreach (var fc in result.Flashcards)
            {
                _db.Flashcards.Add(new Flashcard
                {
                    Id = Guid.NewGuid(),
                    TopicId = request.TopicId,
                    StudentId = request.StudentId,
                    Front = fc.Front,
                    Back = fc.Back,
                    IsAIGenerated = true
                });
            }
            await _db.SaveChangesAsync(ct);

            _ai.SetCache(cacheKey, result, TimeSpan.FromHours(12));
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI flashcard generation failed");
            return AIErrors.ServiceUnavailable;
        }
    }
}
