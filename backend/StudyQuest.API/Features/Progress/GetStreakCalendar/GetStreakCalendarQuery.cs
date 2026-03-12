using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Progress.Common;

namespace StudyQuest.API.Features.Progress.GetStreakCalendar;

public record GetStreakCalendarQuery(Guid StudentId) : IRequest<ErrorOr<StreakCalendarResponse>>;

internal sealed class GetStreakCalendarQueryHandler : IRequestHandler<GetStreakCalendarQuery, ErrorOr<StreakCalendarResponse>>
{
    private readonly AppDbContext _db;

    public GetStreakCalendarQueryHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<StreakCalendarResponse>> Handle(GetStreakCalendarQuery request, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var monthEnd = monthStart.AddMonths(1);

        var studiedDays = await _db.StudySessions
            .Where(s => s.StudentId == request.StudentId
                && s.StartedAt >= monthStart
                && s.StartedAt < monthEnd)
            .Select(s => s.StartedAt.Date.Day)
            .Distinct()
            .OrderBy(d => d)
            .ToListAsync(ct);

        return new StreakCalendarResponse(now.Year, now.Month, studiedDays);
    }
}
