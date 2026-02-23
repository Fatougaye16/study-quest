namespace StudyQuest.API.DTOs.Subjects;

public record SubjectDto(
    Guid Id,
    string Name,
    int Grade,
    string Description,
    string Color,
    int TopicCount
);

public record TopicDto(
    Guid Id,
    Guid SubjectId,
    string Name,
    int Order,
    string Description,
    int NoteCount,
    int QuestionCount
);

public record NoteDto(
    Guid Id,
    Guid TopicId,
    string Title,
    string Content,
    bool IsAIGenerated,
    DateTime CreatedAt
);

public record CreateNoteDto(
    string Title,
    string Content
);

public record QuestionDto(
    Guid Id,
    Guid TopicId,
    string QuestionText,
    string AnswerText,
    int Difficulty,
    bool IsAIGenerated
);

public record CreateQuestionDto(
    string QuestionText,
    string AnswerText,
    int Difficulty
);
