using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyQuest.API.DTOs.StudySessions;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Controllers;

[Route("api/study-sessions")]
[Authorize]
public class StudySessionController : BaseApiController
{
    private readonly IStudySessionService _sessionService;

    public StudySessionController(IStudySessionService sessionService)
    {
        _sessionService = sessionService;
    }

    /// <summary>
    /// Log a new study session.
    /// </summary>
    [HttpPost]
    [ProducesResponseType<StudySessionDto>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSession([FromBody] CreateStudySessionDto dto)
    {
        var session = await _sessionService.CreateSessionAsync(GetStudentId(), dto);
        return CreatedAtAction(nameof(GetSessions), null, session);
    }

    /// <summary>
    /// Get study session history with optional filters.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<List<StudySessionDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSessions(
        [FromQuery] Guid? subjectId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        var sessions = await _sessionService.GetSessionsAsync(GetStudentId(), subjectId, from, to);
        return Ok(sessions);
    }
}
