using ErrorOr;

namespace StudyQuest.API.Features.Enrollments.Common;

public static class EnrollmentErrors
{
    public static Error SubjectNotFound => Error.NotFound(
        code: "Enrollment.SubjectNotFound",
        description: "The requested subject could not be found.");

    public static Error AlreadyEnrolled => Error.Conflict(
        code: "Enrollment.AlreadyEnrolled",
        description: "You are already enrolled in this subject.");

    public static Error EnrollmentNotFound => Error.NotFound(
        code: "Enrollment.NotFound",
        description: "The enrollment could not be found.");
}
