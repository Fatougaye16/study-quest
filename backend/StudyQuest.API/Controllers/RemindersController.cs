using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyQuest.API.DTOs.Reminders;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Controllers;

[Route("api/[controller]")]
[Authorize]
public class RemindersController : BaseApiController
{
    private readonly IReminderService _reminderService;

    public RemindersController(IReminderService reminderService)
    {
        _reminderService = reminderService;
    }

    /// <summary>
    /// Get all reminders for the current student.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<List<ReminderDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReminders()
    {
        var reminders = await _reminderService.GetRemindersAsync(GetStudentId());
        return Ok(reminders);
    }

    /// <summary>
    /// Create a new reminder.
    /// </summary>
    [HttpPost]
    [ProducesResponseType<ReminderDto>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateReminder([FromBody] CreateReminderDto dto)
    {
        var reminder = await _reminderService.CreateReminderAsync(GetStudentId(), dto);
        return CreatedAtAction(nameof(GetReminders), reminder);
    }

    /// <summary>
    /// Delete a reminder.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReminder(Guid id)
    {
        var deleted = await _reminderService.DeleteReminderAsync(GetStudentId(), id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
