using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Subjects.Common;

namespace StudyQuest.API.Features.Subjects.GetNotes;

public record GetNotesQuery(Guid TopicId) : IRequest<ErrorOr<List<NoteResponse>>>;

internal sealed class GetNotesQueryHandler : IRequestHandler<GetNotesQuery, ErrorOr<List<NoteResponse>>>
{
    private readonly AppDbContext _db;

    public GetNotesQueryHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<List<NoteResponse>>> Handle(GetNotesQuery request, CancellationToken ct)
    {
        var notes = await _db.Notes
            .Where(n => n.TopicId == request.TopicId)
            .OrderByDescending(n => n.IsOfficial)
            .ThenByDescending(n => n.CreatedAt)
            .Select(n => new NoteResponse(n.Id, n.TopicId, n.Title, n.Content, n.IsAIGenerated, (int)n.SourceType, n.OriginalFileName, n.IsOfficial, n.CreatedAt))
            .ToListAsync(ct);

        return notes;
    }
}
