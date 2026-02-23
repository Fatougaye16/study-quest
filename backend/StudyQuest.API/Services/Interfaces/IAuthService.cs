using StudyQuest.API.DTOs.Auth;

namespace StudyQuest.API.Services.Interfaces;

public interface IAuthService
{
    Task<bool> RequestOtpAsync(string phoneNumber);
    Task<AuthResponseDto?> VerifyOtpAsync(string phoneNumber, string otpCode);
    Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken);
    Task<bool> LogoutAsync(Guid studentId);
    Task<StudentDto?> GetStudentAsync(Guid studentId);
    Task<StudentDto?> UpdateProfileAsync(Guid studentId, UpdateProfileDto dto);
}
