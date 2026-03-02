using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.StudySessions.Common;

namespace StudyQuest.API.Features.StudySessions.GetSessions;

public record GetSessionsQuery(Guid StudentId, Guid? SubjectId, DateTime? From, DateTime? To)
    : IRequest<ErrorOr<List<StudySessionResponse>>>;

internal sealed class GetSessionsQueryHandler : IRequestHandler<GetSessionsQuery, ErrorOr<List<StudySessionResponse>>>
{
    private readonly AppDbContext _db;

    public GetSessionsQueryHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<List<StudySessionResponse>>> Handle(GetSessionsQuery request, CancellationToken ct)
    {
        var query = _db.StudySessions
            .Where(s => s.StudentId == request.StudentId)
            .Include(s => s.Subject)
            .Include(s => s.Topic)
            .AsQueryable();

        if (request.SubjectId.HasValue)
            query = query.Where(s => s.SubjectId == request.SubjectId.Value);
        if (request.From.HasValue)
            query = query.Where(s => s.StartedAt >= request.From.Value);
        if (request.To.HasValue)
            query = query.Where(s => s.StartedAt <= request.To.Value);

        var sessions = await query
            .OrderByDescending(s => s.StartedAt)
            .Select(s => new StudySessionResponse(
                s.Id, s.SubjectId, s.Subject.Name,
                s.TopicId, s.Topic != null ? s.Topic.Name : null,
                s.StartedAt, s.EndedAt, s.DurationMinutes, s.Notes))
            .ToListAsync(ct);

        return sessions;
    }
}
