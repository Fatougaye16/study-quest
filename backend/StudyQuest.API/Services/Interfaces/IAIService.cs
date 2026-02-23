using StudyQuest.API.DTOs.AI;
using StudyQuest.API.DTOs.StudyPlans;

namespace StudyQuest.API.Services.Interfaces;

public interface IAIService
{
    Task<SummarizeResponseDto> SummarizeAsync(Guid studentId, SummarizeRequestDto request);
    Task<FlashcardResponseDto> GenerateFlashcardsAsync(Guid studentId, FlashcardRequestDto request);
    Task<QuizResponseDto> GenerateQuizAsync(Guid studentId, QuizRequestDto request);
    Task<ExplainResponseDto> ExplainTopicAsync(Guid studentId, ExplainRequestDto request);
    Task<StudyPlanDto> GenerateStudyPlanAsync(Guid studentId, AIStudyPlanRequestDto request);
}
