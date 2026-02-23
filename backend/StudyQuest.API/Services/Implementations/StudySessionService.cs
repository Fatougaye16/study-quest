using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.DTOs.StudySessions;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Services.Implementations;

public class StudySessionService : IStudySessionService
{
    private readonly AppDbContext _db;
    private readonly IProgressService _progressService;

    public StudySessionService(AppDbContext db, IProgressService progressService)
    {
        _db = db;
        _progressService = progressService;
    }

    public async Task<StudySessionDto> CreateSessionAsync(Guid studentId, CreateStudySessionDto dto)
    {
        var subject = await _db.Subjects.FindAsync(dto.SubjectId)
            ?? throw new InvalidOperationException("Subject not found");

        Topic? topic = null;
        if (dto.TopicId.HasValue)
        {
            topic = await _db.Topics.FindAsync(dto.TopicId.Value);
        }

        var session = new StudySession
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            SubjectId = dto.SubjectId,
            TopicId = dto.TopicId,
            StartedAt = dto.StartedAt,
            EndedAt = dto.EndedAt,
            DurationMinutes = dto.DurationMinutes,
            Notes = dto.Notes
        };

        _db.StudySessions.Add(session);
        await _db.SaveChangesAsync();

        // Award XP based on duration (10 XP per 10 minutes, min 10)
        var xp = Math.Max(10, dto.DurationMinutes / 10 * 10);
        await _progressService.AddXPAsync(studentId, dto.SubjectId, xp);

        // Update streak
        await _progressService.UpdateStreakAsync(studentId);

        // Check achievements
        await _progressService.CheckAndUnlockAchievementsAsync(studentId);

        return new StudySessionDto(
            Id: session.Id,
            SubjectId: session.SubjectId,
            SubjectName: subject.Name,
            TopicId: session.TopicId,
            TopicName: topic?.Name,
            StartedAt: session.StartedAt,
            EndedAt: session.EndedAt,
            DurationMinutes: session.DurationMinutes,
            Notes: session.Notes
        );
    }

    public async Task<List<StudySessionDto>> GetSessionsAsync(
        Guid studentId, Guid? subjectId = null, DateTime? from = null, DateTime? to = null)
    {
        var query = _db.StudySessions
            .Where(s => s.StudentId == studentId)
            .Include(s => s.Subject)
            .Include(s => s.Topic)
            .AsQueryable();

        if (subjectId.HasValue)
            query = query.Where(s => s.SubjectId == subjectId.Value);

        if (from.HasValue)
            query = query.Where(s => s.StartedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(s => s.StartedAt <= to.Value);

        return await query
            .OrderByDescending(s => s.StartedAt)
            .Select(s => new StudySessionDto(
                s.Id, s.SubjectId, s.Subject.Name,
                s.TopicId, s.Topic != null ? s.Topic.Name : null,
                s.StartedAt, s.EndedAt, s.DurationMinutes, s.Notes
            ))
            .ToListAsync();
    }
}
