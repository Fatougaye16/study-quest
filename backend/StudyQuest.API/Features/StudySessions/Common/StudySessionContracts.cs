namespace StudyQuest.API.Features.StudySessions.Common;

public record StudySessionResponse(
    Guid Id, Guid SubjectId, string SubjectName,
    Guid? TopicId, string? TopicName,
    DateTime StartedAt, DateTime? EndedAt, int DurationMinutes, string? Notes);

public record CreateStudySessionRequest(
    Guid SubjectId, Guid? TopicId,
    DateTime StartedAt, DateTime? EndedAt,
    int DurationMinutes, string? Notes);
