using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyQuest.API.DTOs.Progress;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Controllers;

[Route("api/[controller]")]
[Authorize]
public class ProgressController : BaseApiController
{
    private readonly IProgressService _progressService;

    public ProgressController(IProgressService progressService)
    {
        _progressService = progressService;
    }

    /// <summary>
    /// Get overall progress including XP, level, streak, and per-subject stats.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<OverallProgressDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProgress()
    {
        var progress = await _progressService.GetOverallProgressAsync(GetStudentId());
        return Ok(progress);
    }

    /// <summary>
    /// Get all achievements with unlock status.
    /// </summary>
    [HttpGet("achievements")]
    [ProducesResponseType<List<AchievementDefinitionDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAchievements()
    {
        var achievements = await _progressService.GetAchievementsAsync(GetStudentId());
        return Ok(achievements);
    }
}
