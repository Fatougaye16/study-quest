using ErrorOr;
using MediatR;
using StudyQuest.API.Data;
using StudyQuest.API.Features.StudySessions.Common;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Features.StudySessions.CreateSession;

public record CreateStudySessionCommand(
    Guid StudentId, Guid SubjectId, Guid? TopicId,
    DateTime StartedAt, DateTime? EndedAt,
    int DurationMinutes, string? Notes) : IRequest<ErrorOr<StudySessionResponse>>;

internal sealed class CreateStudySessionCommandHandler : IRequestHandler<CreateStudySessionCommand, ErrorOr<StudySessionResponse>>
{
    private readonly AppDbContext _db;
    private readonly IProgressService _progressService;

    public CreateStudySessionCommandHandler(AppDbContext db, IProgressService progressService)
    {
        _db = db;
        _progressService = progressService;
    }

    public async Task<ErrorOr<StudySessionResponse>> Handle(CreateStudySessionCommand request, CancellationToken ct)
    {
        var subject = await _db.Subjects.FindAsync([request.SubjectId], ct);
        if (subject is null)
            return StudySessionErrors.SubjectNotFound;

        Topic? topic = null;
        if (request.TopicId.HasValue)
            topic = await _db.Topics.FindAsync([request.TopicId.Value], ct);

        var session = new StudySession
        {
            Id = Guid.NewGuid(),
            StudentId = request.StudentId,
            SubjectId = request.SubjectId,
            TopicId = request.TopicId,
            StartedAt = request.StartedAt,
            EndedAt = request.EndedAt,
            DurationMinutes = request.DurationMinutes,
            Notes = request.Notes
        };

        _db.StudySessions.Add(session);
        await _db.SaveChangesAsync(ct);

        var xp = Math.Max(10, request.DurationMinutes / 10 * 10);
        await _progressService.AddXPAsync(request.StudentId, request.SubjectId, xp);
        await _progressService.UpdateStreakAsync(request.StudentId);
        await _progressService.CheckAndUnlockAchievementsAsync(request.StudentId);

        return new StudySessionResponse(
            session.Id, session.SubjectId, subject.Name,
            session.TopicId, topic?.Name,
            session.StartedAt, session.EndedAt, session.DurationMinutes, session.Notes);
    }
}
