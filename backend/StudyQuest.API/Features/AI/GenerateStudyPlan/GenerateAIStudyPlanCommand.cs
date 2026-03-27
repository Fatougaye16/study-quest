using System.Text.Json;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.AI.Common;
using StudyQuest.API.Features.StudyPlans.Common;
using StudyQuest.API.Models;

namespace StudyQuest.API.Features.AI.GenerateStudyPlan;

public record GenerateAIStudyPlanCommand(Guid StudentId, Guid SubjectId, List<Guid>? TopicIds, int DurationDays)
    : IRequest<ErrorOr<StudyPlanResponse>>;

internal sealed class GenerateAIStudyPlanCommandHandler : IRequestHandler<GenerateAIStudyPlanCommand, ErrorOr<StudyPlanResponse>>
{
    private readonly AppDbContext _db;
    private readonly OpenAIClient _ai;
    private readonly ILogger<GenerateAIStudyPlanCommandHandler> _logger;

    public GenerateAIStudyPlanCommandHandler(AppDbContext db, OpenAIClient ai, ILogger<GenerateAIStudyPlanCommandHandler> logger)
    {
        _db = db;
        _ai = ai;
        _logger = logger;
    }

    public async Task<ErrorOr<StudyPlanResponse>> Handle(GenerateAIStudyPlanCommand request, CancellationToken ct)
    {
        var student = await _db.Students.FindAsync([request.StudentId], ct);
        if (student is null) return AIErrors.StudentNotFound;

        var subject = await _db.Subjects.Include(s => s.Topics)
            .FirstOrDefaultAsync(s => s.Id == request.SubjectId, ct);
        if (subject is null) return AIErrors.SubjectNotFound;

        var topics = request.TopicIds?.Count > 0
            ? subject.Topics.Where(t => request.TopicIds.Contains(t.Id)).OrderBy(t => t.Order).ToList()
            : subject.Topics.OrderBy(t => t.Order).ToList();

        if (topics.Count == 0) return AIErrors.NoTopics;

        var topicList = string.Join("\n", topics.Select((t, i) => $"{i + 1}. {t.Name}"));

        var systemPrompt = $$"""
            {{WASSCEPromptContext.BaseContext}}
            Create a {{request.DurationDays}}-day WASSCE preparation study plan for a Grade {{student.Grade}} student studying {{subject.Name}}.
            Prioritize topics based on their WASSCE exam weight — give more study time to high-frequency and heavily weighted topics.
            Include dedicated revision days and past-question practice sessions.
            Return your response as JSON:
            { "title": "...", "items": [{ "topicIndex": 0, "day": 1, "durationMinutes": 45 }, ...] }
            Rules: topicIndex is 0-based; spread topics evenly; 30-60 minute sessions; include review days.
            Only output valid JSON, nothing else.
            """;

        try
        {
            var response = await _ai.CallAsync(systemPrompt, $"Topics to cover:\n{topicList}", _ai.Model, temperature: 0.3f);

            using var doc = JsonDocument.Parse(response);
            var root = doc.RootElement;
            var title = root.GetProperty("title").GetString() ?? $"{subject.Name} Study Plan";

            var startDate = DateTime.UtcNow.Date;
            var endDate = startDate.AddDays(request.DurationDays);

            var studyPlan = new StudyPlan
            {
                Id = Guid.NewGuid(),
                StudentId = request.StudentId,
                SubjectId = request.SubjectId,
                Title = title,
                StartDate = startDate,
                EndDate = endDate,
                IsAIGenerated = true
            };

            _db.StudyPlans.Add(studyPlan);

            var items = new List<StudyPlanItem>();
            foreach (var item in root.GetProperty("items").EnumerateArray())
            {
                var topicIndex = item.GetProperty("topicIndex").GetInt32();
                var day = item.GetProperty("day").GetInt32();
                var duration = item.GetProperty("durationMinutes").GetInt32();

                if (topicIndex >= 0 && topicIndex < topics.Count)
                {
                    var planItem = new StudyPlanItem
                    {
                        Id = Guid.NewGuid(),
                        StudyPlanId = studyPlan.Id,
                        TopicId = topics[topicIndex].Id,
                        ScheduledDate = startDate.AddDays(day - 1),
                        DurationMinutes = duration,
                        IsCompleted = false
                    };
                    items.Add(planItem);
                    _db.StudyPlanItems.Add(planItem);
                }
            }

            await _db.SaveChangesAsync(ct);

            return new StudyPlanResponse(
                Id: studyPlan.Id,
                SubjectId: subject.Id,
                SubjectName: subject.Name,
                Title: title,
                StartDate: startDate,
                EndDate: endDate,
                IsAIGenerated: true,
                CreatedAt: studyPlan.CreatedAt,
                Items: items.Select(i => new StudyPlanItemResponse(
                    Id: i.Id,
                    TopicId: i.TopicId,
                    TopicName: topics.First(t => t.Id == i.TopicId).Name,
                    ScheduledDate: i.ScheduledDate,
                    DurationMinutes: i.DurationMinutes,
                    IsCompleted: false,
                    CompletedAt: null)).ToList(),
                CompletionPercentage: 0);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI study plan generation failed");
            return AIErrors.ServiceUnavailable;
        }
    }
}
