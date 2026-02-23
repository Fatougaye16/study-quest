namespace StudyQuest.API.Models;

public enum ReminderType
{
    StudyReminder,
    StreakReminder,
    TimetableReminder,
    Custom
}

public class Reminder
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }
    public DateTime? SentAt { get; set; }
    public ReminderType Type { get; set; } = ReminderType.Custom;
    public bool IsRecurring { get; set; }

    // Navigation properties
    public Student Student { get; set; } = null!;
}
