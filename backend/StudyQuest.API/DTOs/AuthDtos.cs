namespace StudyQuest.API.DTOs.Auth;

public record RequestOtpDto(string PhoneNumber);

public record VerifyOtpDto(string PhoneNumber, string OtpCode);

public record RefreshTokenDto(string RefreshToken);

public record AuthResponseDto(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    StudentDto Student
);

public record StudentDto(
    Guid Id,
    string PhoneNumber,
    string FirstName,
    string LastName,
    int Grade,
    int DailyGoalMinutes,
    DateTime CreatedAt
);

public record UpdateProfileDto(
    string? FirstName,
    string? LastName,
    int? Grade,
    int? DailyGoalMinutes
);
