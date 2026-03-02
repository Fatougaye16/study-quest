using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Services.Implementations;

public class ReminderService : IReminderService
{
    private readonly AppDbContext _db;
    private readonly INotificationService _notificationService;
    private readonly ILogger<ReminderService> _logger;

    public ReminderService(AppDbContext db, INotificationService notificationService, ILogger<ReminderService> logger)
    {
        _db = db;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task ProcessDueRemindersAsync()
    {
        var now = DateTime.UtcNow;
        var dueReminders = await _db.Reminders
            .Where(r => r.ScheduledAt <= now && r.SentAt == null)
            .ToListAsync();

        foreach (var reminder in dueReminders)
        {
            try
            {
                await _notificationService.SendPushNotificationAsync(
                    reminder.StudentId, reminder.Title, reminder.Message);

                reminder.SentAt = DateTime.UtcNow;

                // If recurring, schedule next occurrence (next day at same time)
                if (reminder.IsRecurring)
                {
                    _db.Reminders.Add(new Reminder
                    {
                        Id = Guid.NewGuid(),
                        StudentId = reminder.StudentId,
                        Title = reminder.Title,
                        Message = reminder.Message,
                        ScheduledAt = reminder.ScheduledAt.AddDays(1),
                        Type = reminder.Type,
                        IsRecurring = true
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send reminder {ReminderId}", reminder.Id);
            }
        }

        await _db.SaveChangesAsync();
    }
}
