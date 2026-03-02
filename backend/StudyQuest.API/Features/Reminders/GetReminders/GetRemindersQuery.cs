using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Reminders.Common;

namespace StudyQuest.API.Features.Reminders.GetReminders;

public record GetRemindersQuery(Guid StudentId) : IRequest<ErrorOr<List<ReminderResponse>>>;

internal sealed class GetRemindersQueryHandler : IRequestHandler<GetRemindersQuery, ErrorOr<List<ReminderResponse>>>
{
    private readonly AppDbContext _db;

    public GetRemindersQueryHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<List<ReminderResponse>>> Handle(GetRemindersQuery request, CancellationToken ct)
    {
        var reminders = await _db.Reminders
            .Where(r => r.StudentId == request.StudentId && r.ScheduledAt >= DateTime.UtcNow)
            .OrderBy(r => r.ScheduledAt)
            .Select(r => new ReminderResponse(
                r.Id, r.Title, r.Message, r.ScheduledAt, r.SentAt, r.Type.ToString(), r.IsRecurring))
            .ToListAsync(ct);

        return reminders;
    }
}
