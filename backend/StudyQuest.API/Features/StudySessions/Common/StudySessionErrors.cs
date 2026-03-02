using ErrorOr;

namespace StudyQuest.API.Features.StudySessions.Common;

public static class StudySessionErrors
{
    public static Error SubjectNotFound => Error.NotFound(
        code: "StudySession.SubjectNotFound",
        description: "The requested subject could not be found.");
}
