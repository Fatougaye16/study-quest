using FluentValidation;

namespace StudyQuest.API.Features.Reminders.CreateReminder;

public sealed class CreateReminderCommandValidator : AbstractValidator<CreateReminderCommand>
{
    public CreateReminderCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200);

        RuleFor(x => x.Message)
            .MaximumLength(500);

        RuleFor(x => x.ScheduledAt)
            .GreaterThan(DateTime.UtcNow).WithMessage("Scheduled time must be in the future.");

        RuleFor(x => x.Type)
            .NotEmpty()
            .Must(t => t is "StudyReminder" or "StreakReminder" or "TimetableReminder" or "Custom")
            .WithMessage("Type must be StudyReminder, StreakReminder, TimetableReminder, or Custom.");
    }
}
