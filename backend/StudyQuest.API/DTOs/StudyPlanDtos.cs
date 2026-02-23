namespace StudyQuest.API.DTOs.StudyPlans;

public record StudyPlanDto(
    Guid Id,
    Guid SubjectId,
    string SubjectName,
    string Title,
    DateTime StartDate,
    DateTime EndDate,
    bool IsAIGenerated,
    DateTime CreatedAt,
    List<StudyPlanItemDto> Items,
    double CompletionPercentage
);

public record StudyPlanItemDto(
    Guid Id,
    Guid TopicId,
    string TopicName,
    DateTime ScheduledDate,
    int DurationMinutes,
    bool IsCompleted,
    DateTime? CompletedAt
);

public record CreateStudyPlanDto(
    Guid SubjectId,
    string Title,
    DateTime StartDate,
    DateTime EndDate,
    List<CreateStudyPlanItemDto> Items
);

public record CreateStudyPlanItemDto(
    Guid TopicId,
    DateTime ScheduledDate,
    int DurationMinutes
);

public record GenerateStudyPlanDto(
    Guid SubjectId,
    int DurationDays,
    List<Guid>? TopicIds // null = all topics
);
