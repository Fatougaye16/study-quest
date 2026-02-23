using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyQuest.API.DTOs.Timetable;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Controllers;

[Route("api/[controller]")]
[Authorize]
public class TimetableController : BaseApiController
{
    private readonly ITimetableService _timetableService;

    public TimetableController(ITimetableService timetableService)
    {
        _timetableService = timetableService;
    }

    /// <summary>
    /// Get the student's full weekly timetable.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<List<TimetableEntryDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTimetable()
    {
        var timetable = await _timetableService.GetTimetableAsync(GetStudentId());
        return Ok(timetable);
    }

    /// <summary>
    /// Add a new timetable entry.
    /// </summary>
    [HttpPost]
    [ProducesResponseType<TimetableEntryDto>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateEntry([FromBody] CreateTimetableEntryDto dto)
    {
        var entry = await _timetableService.CreateEntryAsync(GetStudentId(), dto);
        return CreatedAtAction(nameof(GetTimetable), null, entry);
    }

    /// <summary>
    /// Update a timetable entry.
    /// </summary>
    [HttpPut("{entryId:guid}")]
    [ProducesResponseType<TimetableEntryDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateEntry(Guid entryId, [FromBody] UpdateTimetableEntryDto dto)
    {
        var result = await _timetableService.UpdateEntryAsync(GetStudentId(), entryId, dto);
        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Delete a timetable entry.
    /// </summary>
    [HttpDelete("{entryId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEntry(Guid entryId)
    {
        var result = await _timetableService.DeleteEntryAsync(GetStudentId(), entryId);
        return result ? NoContent() : NotFound();
    }
}
