namespace StudyQuest.API.Features.StudyPlans.Common;

public record StudyPlanResponse(
    Guid Id, Guid SubjectId, string SubjectName, string Title,
    DateTime StartDate, DateTime EndDate, bool IsAIGenerated,
    DateTime CreatedAt, List<StudyPlanItemResponse> Items, double CompletionPercentage);

public record StudyPlanItemResponse(
    Guid Id, Guid TopicId, string TopicName,
    DateTime ScheduledDate, int DurationMinutes, bool IsCompleted, DateTime? CompletedAt);

public record CreateStudyPlanRequest(
    Guid SubjectId, string Title, DateTime StartDate, DateTime EndDate,
    List<CreateStudyPlanItemRequest> Items);

public record CreateStudyPlanItemRequest(Guid TopicId, DateTime ScheduledDate, int DurationMinutes);
