using StudyQuest.API.Models;

namespace StudyQuest.API.Features.StudyPlans.Common;

public static class StudyPlanMappings
{
    public static StudyPlanResponse ToResponse(this StudyPlan plan)
    {
        var totalItems = plan.Items.Count;
        var completedItems = plan.Items.Count(i => i.IsCompleted);

        return new StudyPlanResponse(
            Id: plan.Id,
            SubjectId: plan.SubjectId,
            SubjectName: plan.Subject.Name,
            Title: plan.Title,
            StartDate: plan.StartDate,
            EndDate: plan.EndDate,
            IsAIGenerated: plan.IsAIGenerated,
            CreatedAt: plan.CreatedAt,
            Items: plan.Items.OrderBy(i => i.ScheduledDate).Select(i => new StudyPlanItemResponse(
                Id: i.Id,
                TopicId: i.TopicId,
                TopicName: i.Topic.Name,
                ScheduledDate: i.ScheduledDate,
                DurationMinutes: i.DurationMinutes,
                IsCompleted: i.IsCompleted,
                CompletedAt: i.CompletedAt
            )).ToList(),
            CompletionPercentage: totalItems > 0 ? Math.Round((double)completedItems / totalItems * 100, 1) : 0);
    }
}
