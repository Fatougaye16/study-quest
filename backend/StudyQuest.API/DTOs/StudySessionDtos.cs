namespace StudyQuest.API.DTOs.StudySessions;

public record StudySessionDto(
    Guid Id,
    Guid SubjectId,
    string SubjectName,
    Guid? TopicId,
    string? TopicName,
    DateTime StartedAt,
    DateTime? EndedAt,
    int DurationMinutes,
    string? Notes
);

public record CreateStudySessionDto(
    Guid SubjectId,
    Guid? TopicId,
    DateTime StartedAt,
    DateTime? EndedAt,
    int DurationMinutes,
    string? Notes
);
