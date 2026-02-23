using StudyQuest.API.DTOs.Progress;

namespace StudyQuest.API.Services.Interfaces;

public interface IProgressService
{
    Task<OverallProgressDto> GetOverallProgressAsync(Guid studentId);
    Task<List<AchievementDefinitionDto>> GetAchievementsAsync(Guid studentId);
    Task AddXPAsync(Guid studentId, Guid subjectId, int amount);
    Task UpdateStreakAsync(Guid studentId);
    Task CheckAndUnlockAchievementsAsync(Guid studentId);
}
