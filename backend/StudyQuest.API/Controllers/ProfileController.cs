using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyQuest.API.DTOs.Auth;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Controllers;

[Route("api/[controller]")]
[Authorize]
public class ProfileController : BaseApiController
{
    private readonly IAuthService _authService;

    public ProfileController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Get the current student's profile.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<StudentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile()
    {
        var student = await _authService.GetStudentAsync(GetStudentId());
        if (student == null) return NotFound();
        return Ok(student);
    }

    /// <summary>
    /// Update the current student's profile (name, grade, daily goal).
    /// </summary>
    [HttpPut]
    [ProducesResponseType<StudentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        var student = await _authService.UpdateProfileAsync(GetStudentId(), dto);
        if (student == null) return NotFound();
        return Ok(student);
    }
}
