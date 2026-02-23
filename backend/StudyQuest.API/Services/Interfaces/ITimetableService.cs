using StudyQuest.API.DTOs.Timetable;

namespace StudyQuest.API.Services.Interfaces;

public interface ITimetableService
{
    Task<List<TimetableEntryDto>> GetTimetableAsync(Guid studentId);
    Task<TimetableEntryDto> CreateEntryAsync(Guid studentId, CreateTimetableEntryDto dto);
    Task<TimetableEntryDto?> UpdateEntryAsync(Guid studentId, Guid entryId, UpdateTimetableEntryDto dto);
    Task<bool> DeleteEntryAsync(Guid studentId, Guid entryId);
}
