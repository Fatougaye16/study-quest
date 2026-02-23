using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyQuest.API.DTOs.StudyPlans;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Controllers;

[Route("api/study-plans")]
[Authorize]
public class StudyPlanController : BaseApiController
{
    private readonly IStudyPlanService _studyPlanService;

    public StudyPlanController(IStudyPlanService studyPlanService)
    {
        _studyPlanService = studyPlanService;
    }

    /// <summary>
    /// Get all study plans for the student.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<List<StudyPlanDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudyPlans()
    {
        var plans = await _studyPlanService.GetStudyPlansAsync(GetStudentId());
        return Ok(plans);
    }

    /// <summary>
    /// Get a specific study plan by ID.
    /// </summary>
    [HttpGet("{planId:guid}")]
    [ProducesResponseType<StudyPlanDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudyPlan(Guid planId)
    {
        var plan = await _studyPlanService.GetStudyPlanByIdAsync(GetStudentId(), planId);
        return plan != null ? Ok(plan) : NotFound();
    }

    /// <summary>
    /// Create a new manual study plan.
    /// </summary>
    [HttpPost]
    [ProducesResponseType<StudyPlanDto>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateStudyPlan([FromBody] CreateStudyPlanDto dto)
    {
        var plan = await _studyPlanService.CreateStudyPlanAsync(GetStudentId(), dto);
        return CreatedAtAction(nameof(GetStudyPlan), new { planId = plan.Id }, plan);
    }

    /// <summary>
    /// Toggle completion status of a study plan item.
    /// </summary>
    [HttpPut("{planId:guid}/items/{itemId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleItemCompletion(Guid planId, Guid itemId)
    {
        var result = await _studyPlanService.ToggleItemCompletionAsync(GetStudentId(), planId, itemId);
        return result ? Ok(new { message = "Item toggled" }) : NotFound();
    }

    /// <summary>
    /// Delete a study plan.
    /// </summary>
    [HttpDelete("{planId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteStudyPlan(Guid planId)
    {
        var result = await _studyPlanService.DeleteStudyPlanAsync(GetStudentId(), planId);
        return result ? NoContent() : NotFound();
    }
}
