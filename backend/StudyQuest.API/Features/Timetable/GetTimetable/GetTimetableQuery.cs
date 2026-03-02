using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Timetable.Common;

namespace StudyQuest.API.Features.Timetable.GetTimetable;

public record GetTimetableQuery(Guid StudentId) : IRequest<ErrorOr<List<TimetableEntryResponse>>>;

internal sealed class GetTimetableQueryHandler : IRequestHandler<GetTimetableQuery, ErrorOr<List<TimetableEntryResponse>>>
{
    private readonly AppDbContext _db;

    public GetTimetableQueryHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<List<TimetableEntryResponse>>> Handle(GetTimetableQuery request, CancellationToken ct)
    {
        var entries = await _db.TimetableEntries
            .Where(t => t.StudentId == request.StudentId)
            .Include(t => t.Subject)
            .OrderBy(t => t.DayOfWeek).ThenBy(t => t.StartTime)
            .Select(t => new TimetableEntryResponse(
                t.Id, t.SubjectId, t.Subject.Name, t.Subject.Color,
                t.DayOfWeek, t.StartTime, t.EndTime, t.Location))
            .ToListAsync(ct);

        return entries;
    }
}
