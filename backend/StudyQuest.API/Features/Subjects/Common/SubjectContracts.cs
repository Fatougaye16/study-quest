namespace StudyQuest.API.Features.Subjects.Common;

public record SubjectResponse(Guid Id, string Name, int Grade, string Description, string Color, int TopicCount);

public record TopicResponse(Guid Id, Guid SubjectId, string Name, int Order, string Description, int NoteCount, int QuestionCount);

public record NoteResponse(Guid Id, Guid TopicId, string Title, string Content, bool IsAIGenerated, DateTime CreatedAt);

public record CreateNoteRequest(string Title, string Content);

public record QuestionResponse(Guid Id, Guid TopicId, string QuestionText, string AnswerText, int Difficulty, bool IsAIGenerated);

public record CreateQuestionRequest(string QuestionText, string AnswerText, int Difficulty);
