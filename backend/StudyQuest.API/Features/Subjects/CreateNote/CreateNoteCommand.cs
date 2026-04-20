using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Subjects.Common;
using StudyQuest.API.Models;

namespace StudyQuest.API.Features.Subjects.CreateNote;

public record CreateNoteCommand(Guid TopicId, string Title, string Content) : IRequest<ErrorOr<NoteResponse>>;

internal sealed class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, ErrorOr<NoteResponse>>
{
    private readonly AppDbContext _db;

    public CreateNoteCommandHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<NoteResponse>> Handle(CreateNoteCommand request, CancellationToken ct)
    {
        var topic = await _db.Topics.FindAsync([request.TopicId], ct);
        if (topic is null)
            return SubjectErrors.TopicNotFound;

        var note = new Note
        {
            Id = Guid.NewGuid(),
            TopicId = request.TopicId,
            Title = request.Title,
            Content = request.Content,
            IsAIGenerated = false
        };

        _db.Notes.Add(note);
        await _db.SaveChangesAsync(ct);

        // Invalidate AI cache for this topic (content changed)
        await _db.CachedAIContents.Where(c => c.TopicId == request.TopicId).ExecuteDeleteAsync(ct);
        await _db.CachedDownloads.Where(c => c.ContentType == DownloadContentType.Notes && c.SourceId == request.TopicId).ExecuteDeleteAsync(ct);

        return new NoteResponse(note.Id, note.TopicId, note.Title, note.Content, note.IsAIGenerated, (int)note.SourceType, note.OriginalFileName, note.IsOfficial, note.CreatedAt);
    }
}
