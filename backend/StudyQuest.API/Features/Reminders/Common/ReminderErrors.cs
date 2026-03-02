using ErrorOr;

namespace StudyQuest.API.Features.Reminders.Common;

public static class ReminderErrors
{
    public static Error NotFound => Error.NotFound(
        code: "Reminder.NotFound",
        description: "Reminder was not found.");
}
