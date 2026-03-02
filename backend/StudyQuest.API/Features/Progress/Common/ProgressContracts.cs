namespace StudyQuest.API.Features.Progress.Common;

public record OverallProgressResponse(
    int TotalXP, int Level, int CurrentStreak,
    int TotalStudyMinutes, int TotalSessions,
    int SubjectsEnrolled, List<SubjectProgressResponse> SubjectProgress);

public record SubjectProgressResponse(
    Guid SubjectId, string SubjectName, string SubjectColor,
    int XP, int Level, int Streak, int TotalStudyMinutes,
    int CompletedTopics, int TotalTopics, double CompletionPercentage);

public record AchievementDefinitionResponse(
    string Type, string Title, string Description,
    string Icon, int XPReward, bool IsUnlocked, DateTime? UnlockedAt);
