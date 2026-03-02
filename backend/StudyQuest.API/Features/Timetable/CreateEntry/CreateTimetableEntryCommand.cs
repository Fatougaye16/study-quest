using ErrorOr;
using MediatR;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Timetable.Common;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Features.Timetable.CreateEntry;

public record CreateTimetableEntryCommand(
    Guid StudentId, Guid SubjectId, DayOfWeek DayOfWeek,
    TimeOnly StartTime, TimeOnly EndTime, string? Location) : IRequest<ErrorOr<TimetableEntryResponse>>;

internal sealed class CreateTimetableEntryCommandHandler : IRequestHandler<CreateTimetableEntryCommand, ErrorOr<TimetableEntryResponse>>
{
    private readonly AppDbContext _db;
    private readonly IProgressService _progressService;

    public CreateTimetableEntryCommandHandler(AppDbContext db, IProgressService progressService)
    {
        _db = db;
        _progressService = progressService;
    }

    public async Task<ErrorOr<TimetableEntryResponse>> Handle(CreateTimetableEntryCommand request, CancellationToken ct)
    {
        var subject = await _db.Subjects.FindAsync([request.SubjectId], ct);
        if (subject is null)
            return TimetableErrors.SubjectNotFound;

        var entry = new TimetableEntry
        {
            Id = Guid.NewGuid(),
            StudentId = request.StudentId,
            SubjectId = request.SubjectId,
            DayOfWeek = request.DayOfWeek,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Location = request.Location
        };

        _db.TimetableEntries.Add(entry);
        await _db.SaveChangesAsync(ct);

        await _progressService.AddXPAsync(request.StudentId, request.SubjectId, 20);

        return new TimetableEntryResponse(
            entry.Id, entry.SubjectId, subject.Name, subject.Color,
            entry.DayOfWeek, entry.StartTime, entry.EndTime, entry.Location);
    }
}
