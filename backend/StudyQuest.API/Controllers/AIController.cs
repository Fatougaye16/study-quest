using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyQuest.API.DTOs.AI;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Controllers;

[Route("api/[controller]")]
[Authorize]
public class AIController : BaseApiController
{
    private readonly IAIService _aiService;

    public AIController(IAIService aiService)
    {
        _aiService = aiService;
    }

    /// <summary>
    /// Summarize notes for a topic into key points.
    /// </summary>
    [HttpPost("summarize")]
    [ProducesResponseType<SummarizeResponseDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Summarize([FromBody] SummarizeRequestDto request)
    {
        try
        {
            var result = await _aiService.SummarizeAsync(GetStudentId(), request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Generate flashcards from a topic's content.
    /// </summary>
    [HttpPost("flashcards")]
    [ProducesResponseType<FlashcardResponseDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GenerateFlashcards([FromBody] FlashcardRequestDto request)
    {
        try
        {
            var result = await _aiService.GenerateFlashcardsAsync(GetStudentId(), request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Generate a quiz for a topic with multiple-choice questions.
    /// </summary>
    [HttpPost("quiz")]
    [ProducesResponseType<QuizResponseDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GenerateQuiz([FromBody] QuizRequestDto request)
    {
        try
        {
            var result = await _aiService.GenerateQuizAsync(GetStudentId(), request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get an AI explanation for a topic, tailored to the student's grade level.
    /// </summary>
    [HttpPost("explain")]
    [ProducesResponseType<ExplainResponseDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ExplainTopic([FromBody] ExplainRequestDto request)
    {
        try
        {
            var result = await _aiService.ExplainTopicAsync(GetStudentId(), request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Generate an AI-powered study plan for a subject.
    /// </summary>
    [HttpPost("study-plan")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GenerateStudyPlan([FromBody] AIStudyPlanRequestDto request)
    {
        try
        {
            var result = await _aiService.GenerateStudyPlanAsync(GetStudentId(), request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
