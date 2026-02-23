namespace StudyQuest.API.DTOs.Reminders;

public record ReminderDto(
    Guid Id,
    string Title,
    string Message,
    DateTime ScheduledAt,
    DateTime? SentAt,
    string Type,
    bool IsRecurring
);

public record CreateReminderDto(
    string Title,
    string Message,
    DateTime ScheduledAt,
    string Type = "Custom",
    bool IsRecurring = false
);
