using StudyQuest.API.DTOs.Reminders;

namespace StudyQuest.API.Services.Interfaces;

public interface IReminderService
{
    Task<List<ReminderDto>> GetRemindersAsync(Guid studentId);
    Task<ReminderDto> CreateReminderAsync(Guid studentId, CreateReminderDto dto);
    Task<bool> DeleteReminderAsync(Guid studentId, Guid reminderId);
    Task ProcessDueRemindersAsync(); // Called by background service
}
