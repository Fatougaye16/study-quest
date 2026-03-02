namespace StudyQuest.API.Features.AI.Common;

// ── Summarize ──
public record SummarizeRequest(Guid TopicId, string? Content, int? Grade);
public record SummarizeResponse(string Summary, List<string> KeyPoints);

// ── Flashcards ──
public record FlashcardRequest(Guid TopicId, string? Content = null, int Count = 10);
public record FlashcardResponse(List<FlashcardItem> Flashcards);
public record FlashcardItem(string Front, string Back);

// ── Quiz ──
public record QuizRequest(Guid TopicId, int? Difficulty = null, int QuestionCount = 10);
public record QuizResponse(List<QuizQuestionItem> Questions);
public record QuizQuestionItem(string Question, List<string> Options, string CorrectAnswer, string Explanation);

// ── Explain ──
public record ExplainRequest(Guid TopicId, string? SpecificQuestion, int? Grade);
public record ExplainResponse(string Explanation, List<string> Examples, List<string> KeyTakeaways);

// ── AI Study Plan ──
public record AIStudyPlanRequest(Guid SubjectId, List<Guid>? TopicIds = null, int DurationDays = 14);
