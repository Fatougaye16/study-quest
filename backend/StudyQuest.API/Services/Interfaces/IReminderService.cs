namespace StudyQuest.API.Services.Interfaces;

public interface IReminderService
{
    Task ProcessDueRemindersAsync(); // Called by background service
}
