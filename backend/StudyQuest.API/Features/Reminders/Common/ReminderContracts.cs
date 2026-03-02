namespace StudyQuest.API.Features.Reminders.Common;

public record ReminderResponse(
    Guid Id,
    string Title,
    string Message,
    DateTime ScheduledAt,
    DateTime? SentAt,
    string Type,
    bool IsRecurring);

public record CreateReminderRequest(
    string Title,
    string Message,
    DateTime ScheduledAt,
    string Type = "Custom",
    bool IsRecurring = false);
