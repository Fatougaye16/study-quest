using ErrorOr;

namespace StudyQuest.API.Features.QuestionBank.Common;

public static class QuestionBankErrors
{
    public static Error StudentNotFound => Error.NotFound(
        code: "QuestionBank.StudentNotFound",
        description: "Student could not be found.");

    public static Error NotAdmin => Error.Forbidden(
        code: "QuestionBank.NotAdmin",
        description: "Only admins can manage the question bank.");

    public static Error PaperNotFound => Error.NotFound(
        code: "QuestionBank.PaperNotFound",
        description: "The requested past paper could not be found.");

    public static Error SubjectNotFound => Error.NotFound(
        code: "QuestionBank.SubjectNotFound",
        description: "The requested subject could not be found.");

    public static Error PaperAlreadyExists => Error.Conflict(
        code: "QuestionBank.PaperAlreadyExists",
        description: "A past paper with the same subject, year, exam type, and paper number already exists.");
}
