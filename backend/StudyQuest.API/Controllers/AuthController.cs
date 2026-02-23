using Microsoft.AspNetCore.Mvc;
using StudyQuest.API.DTOs.Auth;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Controllers;

[Route("api/[controller]")]
public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Request an OTP code sent via SMS to the given phone number.
    /// </summary>
    [HttpPost("request-otp")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> RequestOtp([FromBody] RequestOtpDto dto)
    {
        var result = await _authService.RequestOtpAsync(dto.PhoneNumber);

        if (!result)
            return StatusCode(StatusCodes.Status429TooManyRequests,
                new { message = "Too many OTP requests. Please wait and try again." });

        return Ok(new { message = "OTP sent successfully" });
    }

    /// <summary>
    /// Verify OTP code and receive JWT tokens. Creates account if new user.
    /// </summary>
    [HttpPost("verify-otp")]
    [ProducesResponseType<AuthResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
    {
        var result = await _authService.VerifyOtpAsync(dto.PhoneNumber, dto.OtpCode);

        if (result == null)
            return Unauthorized(new { message = "Invalid or expired OTP code" });

        return Ok(result);
    }

    /// <summary>
    /// Refresh an expired access token using a valid refresh token.
    /// </summary>
    [HttpPost("refresh")]
    [ProducesResponseType<AuthResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
    {
        var result = await _authService.RefreshTokenAsync(dto.RefreshToken);

        if (result == null)
            return Unauthorized(new { message = "Invalid or expired refresh token" });

        return Ok(result);
    }

    /// <summary>
    /// Logout and invalidate the refresh token.
    /// </summary>
    [HttpPost("logout")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync(GetStudentId());
        return Ok(new { message = "Logged out successfully" });
    }
}
