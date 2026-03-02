using System.Text.Json;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.AI.Common;

namespace StudyQuest.API.Features.AI.GenerateQuiz;

public record GenerateQuizCommand(Guid StudentId, Guid TopicId, int? Difficulty, int QuestionCount)
    : IRequest<ErrorOr<QuizResponse>>;

internal sealed class GenerateQuizCommandHandler : IRequestHandler<GenerateQuizCommand, ErrorOr<QuizResponse>>
{
    private readonly AppDbContext _db;
    private readonly OpenAIClient _ai;
    private readonly ILogger<GenerateQuizCommandHandler> _logger;

    public GenerateQuizCommandHandler(AppDbContext db, OpenAIClient ai, ILogger<GenerateQuizCommandHandler> logger)
    {
        _db = db;
        _ai = ai;
        _logger = logger;
    }

    public async Task<ErrorOr<QuizResponse>> Handle(GenerateQuizCommand request, CancellationToken ct)
    {
        var student = await _db.Students.FindAsync([request.StudentId], ct);
        if (student is null) return AIErrors.StudentNotFound;

        var topic = await _db.Topics.Include(t => t.Notes).Include(t => t.Questions)
            .FirstOrDefaultAsync(t => t.Id == request.TopicId, ct);
        if (topic is null) return AIErrors.TopicNotFound;

        var subject = await _db.Subjects.FindAsync([topic.SubjectId], ct);

        var difficultyText = request.Difficulty switch
        {
            1 => "easy (basic recall and understanding)",
            2 => "medium (application and analysis)",
            3 => "hard (synthesis and evaluation)",
            _ => "mixed (varying difficulty levels)"
        };

        var notesContent = string.Join("\n", topic.Notes.Select(n => n.Content));
        var existingQA = string.Join("\n", topic.Questions.Select(q => $"Q: {q.QuestionText} A: {q.AnswerText}"));

        var systemPrompt = $$"""
            You are an educational quiz generator for South African Grade {{student.Grade}} students studying {{subject?.Name ?? "this subject"}}.
            Generate exactly {{request.QuestionCount}} multiple-choice questions about "{{topic.Name}}".
            Difficulty level: {{difficultyText}}
            Return your response as JSON:
            { "questions": [{ "question": "...", "options": ["A","B","C","D"], "correctAnswer": "...", "explanation": "..." }, ...] }
            Only output valid JSON, nothing else.
            """;

        var userContent = $"Topic: {topic.Name}";
        if (!string.IsNullOrWhiteSpace(notesContent)) userContent += $"\n\nStudy Material:\n{notesContent}";
        if (!string.IsNullOrWhiteSpace(existingQA)) userContent += $"\n\nReference Q&A:\n{existingQA}";

        try
        {
            var response = await _ai.CallAsync(systemPrompt, userContent, _ai.Model);
            var result = JsonSerializer.Deserialize<QuizResponse>(response, OpenAIClient.JsonOptions)
                ?? new QuizResponse([]);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI quiz generation failed");
            return AIErrors.ServiceUnavailable;
        }
    }
}
