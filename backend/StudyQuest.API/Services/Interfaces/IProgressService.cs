using StudyQuest.API.Features.Progress.Common;

namespace StudyQuest.API.Services.Interfaces;

public interface IProgressService
{
    Task<OverallProgressResponse> GetOverallProgressAsync(Guid studentId);
    Task<List<AchievementDefinitionResponse>> GetAchievementsAsync(Guid studentId);
    Task AddXPAsync(Guid studentId, Guid subjectId, int amount);
    Task UpdateStreakAsync(Guid studentId);
    Task CheckAndUnlockAchievementsAsync(Guid studentId);
}
