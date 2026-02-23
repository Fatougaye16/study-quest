using StudyQuest.API.DTOs.Subjects;

namespace StudyQuest.API.Services.Interfaces;

public interface ISubjectService
{
    Task<List<SubjectDto>> GetSubjectsByGradeAsync(int grade);
    Task<List<TopicDto>> GetTopicsBySubjectAsync(Guid subjectId);
    Task<List<NoteDto>> GetNotesByTopicAsync(Guid topicId);
    Task<NoteDto> CreateNoteAsync(Guid topicId, CreateNoteDto dto);
    Task<List<QuestionDto>> GetQuestionsByTopicAsync(Guid topicId);
    Task<QuestionDto> CreateQuestionAsync(Guid topicId, CreateQuestionDto dto);
}
