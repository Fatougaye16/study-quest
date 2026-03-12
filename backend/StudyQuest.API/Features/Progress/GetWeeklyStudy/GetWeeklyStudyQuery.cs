using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Progress.Common;

namespace StudyQuest.API.Features.Progress.GetWeeklyStudy;

public record GetWeeklyStudyQuery(Guid StudentId) : IRequest<ErrorOr<List<WeeklyStudyDay>>>;

internal sealed class GetWeeklyStudyQueryHandler : IRequestHandler<GetWeeklyStudyQuery, ErrorOr<List<WeeklyStudyDay>>>
{
    private readonly AppDbContext _db;

    public GetWeeklyStudyQueryHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<List<WeeklyStudyDay>>> Handle(GetWeeklyStudyQuery request, CancellationToken ct)
    {
        var today = DateTime.UtcNow.Date;
        var weekStart = today.AddDays(-6); // Last 7 days including today

        var sessions = await _db.StudySessions
            .Where(s => s.StudentId == request.StudentId && s.StartedAt >= weekStart)
            .ToListAsync(ct);

        var result = new List<WeeklyStudyDay>();

        for (var day = weekStart; day <= today; day = day.AddDays(1))
        {
            var minutes = sessions
                .Where(s => s.StartedAt.Date == day)
                .Sum(s => s.DurationMinutes);

            result.Add(new WeeklyStudyDay(
                DayLabel: day.ToString("ddd"),
                Date: day.ToString("yyyy-MM-dd"),
                Minutes: minutes));
        }

        return result;
    }
}
