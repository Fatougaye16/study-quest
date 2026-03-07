namespace StudyQuest.API.Features.Auth.Common;

public record AuthResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt, StudentResponse Student);

public record StudentResponse(Guid Id, string PhoneNumber, string FirstName, string LastName, int Grade, int DailyGoalMinutes, bool IsOtpEnabled, DateTime CreatedAt);

public record RegisterRequest(string PhoneNumber, string Password, string FirstName, string LastName, int Grade = 10, bool EnableOtp = false);

public record LoginRequest(string PhoneNumber, string Password);

public record LoginOtpRequiredResponse(bool OtpRequired, string Message, string PhoneNumber);

public record UpdateProfileRequest(string? FirstName, string? LastName, int? Grade, int? DailyGoalMinutes);
