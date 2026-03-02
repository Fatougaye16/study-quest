using ErrorOr;

namespace StudyQuest.API.Features.Subjects.Common;

public static class SubjectErrors
{
    public static Error TopicNotFound => Error.NotFound(
        code: "Subject.TopicNotFound",
        description: "The requested topic could not be found.");

    public static Error SubjectNotFound => Error.NotFound(
        code: "Subject.SubjectNotFound",
        description: "The requested subject could not be found.");
}
