using StudyQuest.API.DTOs.StudySessions;

namespace StudyQuest.API.Services.Interfaces;

public interface IStudySessionService
{
    Task<StudySessionDto> CreateSessionAsync(Guid studentId, CreateStudySessionDto dto);
    Task<List<StudySessionDto>> GetSessionsAsync(Guid studentId, Guid? subjectId = null, DateTime? from = null, DateTime? to = null);
}
