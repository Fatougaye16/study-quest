namespace StudyQuest.API.DTOs.Enrollments;

public record EnrollmentDto(
    Guid Id,
    Guid SubjectId,
    string SubjectName,
    string SubjectColor,
    int Grade,
    DateTime EnrolledAt
);

public record CreateEnrollmentDto(
    Guid SubjectId
);

public record RegisterDeviceDto(
    string Token,
    string Platform // "ios" or "android"
);
