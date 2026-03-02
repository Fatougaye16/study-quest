using ErrorOr;

namespace StudyQuest.API.Features.AI.Common;

public static class AIErrors
{
    public static Error StudentNotFound => Error.NotFound(
        code: "AI.StudentNotFound",
        description: "Student could not be found.");

    public static Error TopicNotFound => Error.NotFound(
        code: "AI.TopicNotFound",
        description: "The requested topic could not be found.");

    public static Error SubjectNotFound => Error.NotFound(
        code: "AI.SubjectNotFound",
        description: "The requested subject could not be found.");

    public static Error NoContent => Error.Validation(
        code: "AI.NoContent",
        description: "No content available to process. Add notes or provide content.");

    public static Error NoTopics => Error.Validation(
        code: "AI.NoTopics",
        description: "No topics found for the study plan.");

    public static Error ServiceUnavailable => Error.Failure(
        code: "AI.ServiceUnavailable",
        description: "AI service is temporarily unavailable. Please try again later.");
}
