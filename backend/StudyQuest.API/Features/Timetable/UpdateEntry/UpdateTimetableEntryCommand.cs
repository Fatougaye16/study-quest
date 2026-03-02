using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Timetable.Common;

namespace StudyQuest.API.Features.Timetable.UpdateEntry;

public record UpdateTimetableEntryCommand(
    Guid StudentId, Guid EntryId, Guid SubjectId,
    DayOfWeek DayOfWeek, TimeOnly StartTime, TimeOnly EndTime, string? Location) : IRequest<ErrorOr<TimetableEntryResponse>>;

internal sealed class UpdateTimetableEntryCommandHandler : IRequestHandler<UpdateTimetableEntryCommand, ErrorOr<TimetableEntryResponse>>
{
    private readonly AppDbContext _db;

    public UpdateTimetableEntryCommandHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<TimetableEntryResponse>> Handle(UpdateTimetableEntryCommand request, CancellationToken ct)
    {
        var entry = await _db.TimetableEntries
            .Include(t => t.Subject)
            .FirstOrDefaultAsync(t => t.Id == request.EntryId && t.StudentId == request.StudentId, ct);

        if (entry is null)
            return TimetableErrors.EntryNotFound;

        var subject = await _db.Subjects.FindAsync([request.SubjectId], ct);
        if (subject is null)
            return TimetableErrors.SubjectNotFound;

        entry.SubjectId = request.SubjectId;
        entry.DayOfWeek = request.DayOfWeek;
        entry.StartTime = request.StartTime;
        entry.EndTime = request.EndTime;
        entry.Location = request.Location;

        await _db.SaveChangesAsync(ct);

        return new TimetableEntryResponse(
            entry.Id, entry.SubjectId, subject.Name, subject.Color,
            entry.DayOfWeek, entry.StartTime, entry.EndTime, entry.Location);
    }
}
