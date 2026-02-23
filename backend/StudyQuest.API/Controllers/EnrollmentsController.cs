using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyQuest.API.DTOs.Enrollments;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Controllers;

[Route("api/[controller]")]
[Authorize]
public class EnrollmentsController : BaseApiController
{
    private readonly IEnrollmentService _enrollmentService;
    private readonly INotificationService _notificationService;

    public EnrollmentsController(IEnrollmentService enrollmentService, INotificationService notificationService)
    {
        _enrollmentService = enrollmentService;
        _notificationService = notificationService;
    }

    /// <summary>
    /// Get all subjects the student is enrolled in.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<List<EnrollmentDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEnrollments()
    {
        var enrollments = await _enrollmentService.GetEnrollmentsAsync(GetStudentId());
        return Ok(enrollments);
    }

    /// <summary>
    /// Enroll the student in a subject.
    /// </summary>
    [HttpPost]
    [ProducesResponseType<EnrollmentDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Enroll([FromBody] CreateEnrollmentDto dto)
    {
        try
        {
            var enrollment = await _enrollmentService.EnrollAsync(GetStudentId(), dto);
            return CreatedAtAction(nameof(GetEnrollments), null, enrollment);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Unenroll the student from a subject.
    /// </summary>
    [HttpDelete("{enrollmentId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Unenroll(Guid enrollmentId)
    {
        var result = await _enrollmentService.UnenrollAsync(GetStudentId(), enrollmentId);
        return result ? NoContent() : NotFound();
    }

    /// <summary>
    /// Register a device token for push notifications.
    /// </summary>
    [HttpPost("device-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterDeviceToken([FromBody] RegisterDeviceDto dto)
    {
        await _notificationService.RegisterDeviceTokenAsync(GetStudentId(), dto.Token, dto.Platform);
        return Ok(new { message = "Device registered" });
    }
}
