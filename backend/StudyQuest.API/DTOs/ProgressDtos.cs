namespace StudyQuest.API.DTOs.Progress;

public record OverallProgressDto(
    int TotalXP,
    int Level,
    int CurrentStreak,
    int TotalStudyMinutes,
    int TotalSessions,
    int SubjectsEnrolled,
    List<SubjectProgressDto> SubjectProgress
);

public record SubjectProgressDto(
    Guid SubjectId,
    string SubjectName,
    string SubjectColor,
    int XP,
    int Level,
    int Streak,
    int TotalStudyMinutes,
    int CompletedTopics,
    int TotalTopics,
    double CompletionPercentage
);

public record AchievementDto(
    Guid Id,
    string Type,
    string Title,
    string Description,
    string Icon,
    int XPReward,
    DateTime UnlockedAt
);

public record AchievementDefinitionDto(
    string Type,
    string Title,
    string Description,
    string Icon,
    int XPReward,
    bool IsUnlocked,
    DateTime? UnlockedAt
);
