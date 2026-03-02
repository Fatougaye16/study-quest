using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Progress.Common;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Services.Implementations;

public class ProgressService : IProgressService
{
    private readonly AppDbContext _db;
    private readonly ILogger<ProgressService> _logger;

    // Achievement definitions
    private static readonly List<AchievementDefinition> AchievementDefinitions =
    [
        new("first_steps", "First Steps", "Complete your first study session", "🎯", 50),
        new("getting_organized", "Getting Organized", "Add your first timetable entry", "📅", 30),
        new("study_planner", "Study Planner", "Complete a study plan item", "📋", 40),
        new("dedicated_student", "Dedicated Student", "Study for 10 hours total", "📚", 100),
        new("study_marathon", "Study Marathon", "Study for 50 hours total", "🏃", 250),
        new("week_warrior", "Week Warrior", "Maintain a 7-day streak", "🔥", 150),
        new("knowledge_seeker", "Knowledge Seeker", "Complete 5 study sessions", "🔍", 75),
        new("course_master", "Course Master", "Enroll in 5 subjects", "🎓", 100),
        new("quiz_champion", "Quiz Champion", "Score 100% on a quiz", "🏆", 200),
        new("flashcard_fan", "Flashcard Fan", "Generate 50 flashcards", "🃏", 75),
        new("consistency_king", "Consistency King", "Maintain a 30-day streak", "👑", 500),
        new("ai_explorer", "AI Explorer", "Use AI features 10 times", "🤖", 100),
    ];

    public ProgressService(AppDbContext db, ILogger<ProgressService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<OverallProgressResponse> GetOverallProgressAsync(Guid studentId)
    {
        var progressRecords = await _db.StudentProgress
            .Where(p => p.StudentId == studentId)
            .Include(p => p.Subject)
            .ToListAsync();

        var sessions = await _db.StudySessions
            .Where(s => s.StudentId == studentId)
            .ToListAsync();

        var enrollments = await _db.Enrollments
            .Where(e => e.StudentId == studentId)
            .Include(e => e.Subject)
            .ToListAsync();

        var subjectProgress = new List<SubjectProgressResponse>();

        foreach (var enrollment in enrollments)
        {
            var progress = progressRecords.FirstOrDefault(p => p.SubjectId == enrollment.SubjectId);
            var subjectTopics = await _db.Topics.Where(t => t.SubjectId == enrollment.SubjectId).CountAsync();
            var completedPlanItems = await _db.StudyPlanItems
                .Where(i => i.StudyPlan.StudentId == studentId
                    && i.StudyPlan.SubjectId == enrollment.SubjectId
                    && i.IsCompleted)
                .Select(i => i.TopicId)
                .Distinct()
                .CountAsync();

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
                    ? Math.Round((double)completedPlanItems / subjectTopics * 100, 1)
                    : 0
            ));
        }

        return new OverallProgressResponse(
            TotalXP: progressRecords.Sum(p => p.XP),
            Level: progressRecords.Sum(p => p.XP) / 500 + 1,
            CurrentStreak: progressRecords.DefaultIfEmpty().Max(p => p?.Streak ?? 0),
            TotalStudyMinutes: progressRecords.Sum(p => p.TotalStudyMinutes),
            TotalSessions: sessions.Count,
            SubjectsEnrolled: enrollments.Count,
            SubjectProgress: subjectProgress
        );
    }

    public async Task<List<AchievementDefinitionResponse>> GetAchievementsAsync(Guid studentId)
    {
        var unlockedAchievements = await _db.Achievements
            .Where(a => a.StudentId == studentId)
            .ToListAsync();

        return AchievementDefinitions.Select(def =>
        {
            var unlocked = unlockedAchievements.FirstOrDefault(a => a.Type == def.Type);
            return new AchievementDefinitionResponse(
                Type: def.Type,
                Title: def.Title,
                Description: def.Description,
                Icon: def.Icon,
                XPReward: def.XPReward,
                IsUnlocked: unlocked != null,
                UnlockedAt: unlocked?.UnlockedAt
            );
        }).ToList();
    }

    public async Task AddXPAsync(Guid studentId, Guid subjectId, int amount)
    {
        var progress = await _db.StudentProgress
            .FirstOrDefaultAsync(p => p.StudentId == studentId && p.SubjectId == subjectId);

        if (progress == null)
        {
            progress = new StudentProgress
            {
                Id = Guid.NewGuid(),
                StudentId = studentId,
                SubjectId = subjectId
            };
            _db.StudentProgress.Add(progress);
        }

        progress.AddXP(amount);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateStreakAsync(Guid studentId)
    {
        var progressRecords = await _db.StudentProgress
            .Where(p => p.StudentId == studentId)
            .ToListAsync();

        var today = DateTime.UtcNow.Date;

        foreach (var progress in progressRecords)
        {
            // Get latest session for this subject
            var latestSession = await _db.StudySessions
                .Where(s => s.StudentId == studentId && s.SubjectId == progress.SubjectId)
                .OrderByDescending(s => s.StartedAt)
                .FirstOrDefaultAsync();

            if (latestSession == null) continue;

            if (progress.LastStudyDate == null)
            {
                progress.Streak = 1;
            }
            else
            {
                var lastDate = progress.LastStudyDate.Value.Date;
                if (lastDate == today)
                {
                    // Already studied today, no change
                }
                else if (lastDate == today.AddDays(-1))
                {
                    // Consecutive day
                    progress.Streak++;
                }
                else
                {
                    // Streak broken
                    progress.Streak = 1;
                }
            }

            progress.LastStudyDate = today;

            // Update total study minutes
            var totalMinutes = await _db.StudySessions
                .Where(s => s.StudentId == studentId && s.SubjectId == progress.SubjectId)
                .SumAsync(s => s.DurationMinutes);
            progress.TotalStudyMinutes = totalMinutes;
        }

        await _db.SaveChangesAsync();
    }

    public async Task CheckAndUnlockAchievementsAsync(Guid studentId)
    {
        var existing = await _db.Achievements
            .Where(a => a.StudentId == studentId)
            .Select(a => a.Type)
            .ToListAsync();

        var sessionCount = await _db.StudySessions.CountAsync(s => s.StudentId == studentId);
        var totalMinutes = await _db.StudentProgress
            .Where(p => p.StudentId == studentId)
            .SumAsync(p => p.TotalStudyMinutes);
        var maxStreak = await _db.StudentProgress
            .Where(p => p.StudentId == studentId)
            .Select(p => p.Streak)
            .DefaultIfEmpty(0)
            .MaxAsync();
        var enrollmentCount = await _db.Enrollments.CountAsync(e => e.StudentId == studentId);
        var flashcardCount = await _db.Flashcards.CountAsync(f => f.StudentId == studentId);
        var completedPlanItems = await _db.StudyPlanItems
            .CountAsync(i => i.StudyPlan.StudentId == studentId && i.IsCompleted);

        var checks = new Dictionary<string, bool>
        {
            ["first_steps"] = sessionCount >= 1,
            ["knowledge_seeker"] = sessionCount >= 5,
            ["dedicated_student"] = totalMinutes >= 600,
            ["study_marathon"] = totalMinutes >= 3000,
            ["week_warrior"] = maxStreak >= 7,
            ["consistency_king"] = maxStreak >= 30,
            ["course_master"] = enrollmentCount >= 5,
            ["flashcard_fan"] = flashcardCount >= 50,
            ["study_planner"] = completedPlanItems >= 1,
        };

        foreach (var (type, unlocked) in checks)
        {
            if (unlocked && !existing.Contains(type))
            {
                var def = AchievementDefinitions.First(d => d.Type == type);
                _db.Achievements.Add(new Achievement
                {
                    Id = Guid.NewGuid(),
                    StudentId = studentId,
                    Type = type,
                    Title = def.Title,
                    Description = def.Description,
                    Icon = def.Icon,
                    XPReward = def.XPReward
                });

                _logger.LogInformation("Achievement unlocked for {StudentId}: {Type}", studentId, type);
            }
        }

        await _db.SaveChangesAsync();
    }

    private record AchievementDefinition(string Type, string Title, string Description, string Icon, int XPReward);
}
