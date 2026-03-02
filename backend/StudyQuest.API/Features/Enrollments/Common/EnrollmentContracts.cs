namespace StudyQuest.API.Features.Enrollments.Common;

public record EnrollmentResponse(Guid Id, Guid SubjectId, string SubjectName, string SubjectColor, int Grade, DateTime EnrolledAt);

public record CreateEnrollmentRequest(Guid SubjectId);

public record RegisterDeviceRequest(string Token, string Platform);
