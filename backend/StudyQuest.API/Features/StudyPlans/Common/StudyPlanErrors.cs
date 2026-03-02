using ErrorOr;

namespace StudyQuest.API.Features.StudyPlans.Common;

public static class StudyPlanErrors
{
    public static Error SubjectNotFound => Error.NotFound(
        code: "StudyPlan.SubjectNotFound",
        description: "The requested subject could not be found.");

    public static Error TopicNotFound(Guid topicId) => Error.NotFound(
        code: "StudyPlan.TopicNotFound",
        description: $"Topic {topicId} could not be found.");

    public static Error PlanNotFound => Error.NotFound(
        code: "StudyPlan.NotFound",
        description: "The study plan could not be found.");

    public static Error ItemNotFound => Error.NotFound(
        code: "StudyPlan.ItemNotFound",
        description: "The study plan item could not be found.");
}
