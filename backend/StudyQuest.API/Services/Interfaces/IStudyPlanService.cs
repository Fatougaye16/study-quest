using StudyQuest.API.DTOs.StudyPlans;

namespace StudyQuest.API.Services.Interfaces;

public interface IStudyPlanService
{
    Task<List<StudyPlanDto>> GetStudyPlansAsync(Guid studentId);
    Task<StudyPlanDto?> GetStudyPlanByIdAsync(Guid studentId, Guid planId);
    Task<StudyPlanDto> CreateStudyPlanAsync(Guid studentId, CreateStudyPlanDto dto);
    Task<bool> ToggleItemCompletionAsync(Guid studentId, Guid planId, Guid itemId);
    Task<bool> DeleteStudyPlanAsync(Guid studentId, Guid planId);
}
