using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Timetable.Common;

namespace StudyQuest.API.Features.Timetable.DeleteEntry;

public record DeleteTimetableEntryCommand(Guid StudentId, Guid EntryId) : IRequest<ErrorOr<Unit>>;

internal sealed class DeleteTimetableEntryCommandHandler : IRequestHandler<DeleteTimetableEntryCommand, ErrorOr<Unit>>
{
    private readonly AppDbContext _db;

    public DeleteTimetableEntryCommandHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<Unit>> Handle(DeleteTimetableEntryCommand request, CancellationToken ct)
    {
        var entry = await _db.TimetableEntries
            .FirstOrDefaultAsync(t => t.Id == request.EntryId && t.StudentId == request.StudentId, ct);

        if (entry is null)
            return TimetableErrors.EntryNotFound;

        _db.TimetableEntries.Remove(entry);
        await _db.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
