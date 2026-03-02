using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Reminders.Common;

namespace StudyQuest.API.Features.Reminders.DeleteReminder;

public record DeleteReminderCommand(Guid StudentId, Guid ReminderId) : IRequest<ErrorOr<Deleted>>;

internal sealed class DeleteReminderCommandHandler : IRequestHandler<DeleteReminderCommand, ErrorOr<Deleted>>
{
    private readonly AppDbContext _db;

    public DeleteReminderCommandHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<Deleted>> Handle(DeleteReminderCommand request, CancellationToken ct)
    {
        var reminder = await _db.Reminders
            .FirstOrDefaultAsync(r => r.Id == request.ReminderId && r.StudentId == request.StudentId, ct);

        if (reminder is null) return ReminderErrors.NotFound;

        _db.Reminders.Remove(reminder);
        await _db.SaveChangesAsync(ct);
        return Result.Deleted;
    }
}
