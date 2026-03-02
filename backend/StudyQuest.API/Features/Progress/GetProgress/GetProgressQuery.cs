using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Progress.Common;

namespace StudyQuest.API.Features.Progress.GetProgress;

public record GetProgressQuery(Guid StudentId) : IRequest<ErrorOr<OverallProgressResponse>>;

internal sealed class GetProgressQueryHandler : IRequestHandler<GetProgressQuery, ErrorOr<OverallProgressResponse>>
{
    private readonly AppDbContext _db;

    public GetProgressQueryHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<OverallProgressResponse>> Handle(GetProgressQuery request, CancellationToken ct)
    {
        var progressRecords = await _db.StudentProgress
            .Where(p => p.StudentId == request.StudentId)
            .Include(p => p.Subject)
            .ToListAsync(ct);

        var sessions = await _db.StudySessions
            .Where(s => s.StudentId == request.StudentId)
            .ToListAsync(ct);

        var enrollments = await _db.Enrollments
            .Where(e => e.StudentId == request.StudentId)
            .Include(e => e.Subject)
            .ToListAsync(ct);

        var subjectProgress = new List<SubjectProgressResponse>();

        foreach (var enrollment in enrollments)
        {
            var progress = progressRecords.FirstOrDefault(p => p.SubjectId == enrollment.SubjectId);
            var subjectTopics = await _db.Topics.Where(t => t.SubjectId == enrollment.SubjectId).CountAsync(ct);
            var completedPlanItems = await _db.StudyPlanItems
                .Where(i => i.StudyPlan.StudentId == request.StudentId
                    && i.StudyPlan.SubjectId == enrollment.SubjectId
                    && i.IsCompleted)
                .Select(i => i.TopicId)
                .Distinct()
                .CountAsync(ct);

            subjectProgress.Add(new SubjectProgressResponse(
                SubjectId: enrollment.SubjectId,
                SubjectName: enrollment.Subject.Name,
                SubjectColor: enrollment.Subject.Color,
                XP: progress?.XP ?? 0,
                Level: progress?.Level ?? 1,
                Streak: progress?.Streak ?? 0,
                TotalStudyMinutes: progress?.TotalStudyMinutes ?? 0,
                CompletedTopics: completedPlanItems,
                TotalTopics: subjectTopics,
                CompletionPercentage: subjectTopics > 0
                    ? Math.Round((double)completedPlanItems / subjectTopics * 100, 1) : 0));
        }

        return new OverallProgressResponse(
            TotalXP: progressRecords.Sum(p => p.XP),
            Level: progressRecords.Sum(p => p.XP) / 500 + 1,
            CurrentStreak: progressRecords.DefaultIfEmpty().Max(p => p?.Streak ?? 0),
            TotalStudyMinutes: progressRecords.Sum(p => p.TotalStudyMinutes),
            TotalSessions: sessions.Count,
            SubjectsEnrolled: enrollments.Count,
            SubjectProgress: subjectProgress);
    }
}
