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

        var difficultyText = WASSCEPromptContext.DifficultyMapping(request.Difficulty);

        var notesContent = WASSCEPromptContext.BuildNoteContent(topic.Notes);
        var existingQA = string.Join("\n", topic.Questions.Select(q => $"Q: {q.QuestionText} A: {q.AnswerText}"));

        var systemPrompt = $$"""
            {{WASSCEPromptContext.BaseContext}}
            You are helping a Grade {{student.Grade}} student studying {{subject?.Name ?? "this subject"}}.
            Generate exactly {{request.QuestionCount}} multiple-choice questions about "{{topic.Name}}".
            Model questions after the style of WASSCE examination questions.
            Difficulty level: {{difficultyText}}
            {{WASSCEPromptContext.ExamFormatGuidance}}
            In the explanation for each question, include a brief WASSCE exam tip where relevant.
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
