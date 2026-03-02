using ErrorOr;
using MediatR;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Reminders.Common;
using StudyQuest.API.Models;

namespace StudyQuest.API.Features.Reminders.CreateReminder;

public record CreateReminderCommand(
    Guid StudentId,
    string Title,
    string Message,
    DateTime ScheduledAt,
    string Type,
    bool IsRecurring) : IRequest<ErrorOr<ReminderResponse>>;

internal sealed class CreateReminderCommandHandler : IRequestHandler<CreateReminderCommand, ErrorOr<ReminderResponse>>
{
    private readonly AppDbContext _db;

    public CreateReminderCommandHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<ReminderResponse>> Handle(CreateReminderCommand request, CancellationToken ct)
    {
        var reminder = new Reminder
        {
            Id = Guid.NewGuid(),
            StudentId = request.StudentId,
            Title = request.Title,
            Message = request.Message,
            ScheduledAt = request.ScheduledAt,
            Type = Enum.TryParse<ReminderType>(request.Type, out var type) ? type : ReminderType.Custom,
            IsRecurring = request.IsRecurring
        };

        _db.Reminders.Add(reminder);
        await _db.SaveChangesAsync(ct);

        return new ReminderResponse(
            reminder.Id, reminder.Title, reminder.Message, reminder.ScheduledAt,
            reminder.SentAt, reminder.Type.ToString(), reminder.IsRecurring);
    }
}
