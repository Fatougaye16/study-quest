using ErrorOr;
using MediatR;
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

        return new NoteResponse(note.Id, note.TopicId, note.Title, note.Content, note.IsAIGenerated, note.CreatedAt);
    }
}
