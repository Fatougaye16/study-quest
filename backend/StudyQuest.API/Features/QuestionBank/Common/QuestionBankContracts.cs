namespace StudyQuest.API.Features.QuestionBank.Common;

// ── Responses ──
public record PastPaperResponse(
    Guid Id, Guid SubjectId, string SubjectName, int Year,
    string ExamType, int PaperNumber, string Title, int QuestionCount, DateTime CreatedAt);

public record PastQuestionResponse(
    Guid Id, int QuestionNumber, string QuestionText, string? AnswerText,
    int? Marks, string? ImageUrl, int Difficulty, string? TopicName);

// ── Requests ──
public record CreatePastPaperRequest(Guid SubjectId, int Year, string ExamType, int PaperNumber, string Title);

public record AddPastQuestionsRequest(List<PastQuestionInput> Questions);

public record PastQuestionInput(
    int QuestionNumber, string QuestionText, string? AnswerText,
    int? Marks, Guid? TopicId, int Difficulty = 1);
