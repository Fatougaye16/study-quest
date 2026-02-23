namespace StudyQuest.API.DTOs.AI;

public record SummarizeRequestDto(
    Guid TopicId,
    string? Content, // If null, summarizes the topic's notes
    int? Grade       // If null, uses student's grade
);

public record SummarizeResponseDto(
    string Summary,
    List<string> KeyPoints
);

public record FlashcardRequestDto(
    Guid TopicId,
    string? Content = null, // If null, generates from topic notes
    int Count = 10
);

public record FlashcardResponseDto(
    List<FlashcardItemDto> Flashcards
);

public record FlashcardItemDto(
    string Front,
    string Back
);

public record QuizRequestDto(
    Guid TopicId,
    int? Difficulty = null, // 1-3, null = mixed
    int QuestionCount = 10
);

public record QuizResponseDto(
    List<QuizQuestionItemDto> Questions
);

public record QuizQuestionItemDto(
    string Question,
    List<string> Options,
    string CorrectAnswer,
    string Explanation
);

public record ExplainRequestDto(
    Guid TopicId,
    string? SpecificQuestion, // Optional specific question about the topic
    int? Grade
);

public record ExplainResponseDto(
    string Explanation,
    List<string> Examples,
    List<string> KeyTakeaways
);

public record AIStudyPlanRequestDto(
    Guid SubjectId,
    List<Guid>? TopicIds = null,
    int DurationDays = 14
);
