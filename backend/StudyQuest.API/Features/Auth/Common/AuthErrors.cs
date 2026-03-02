using ErrorOr;

namespace StudyQuest.API.Features.Auth.Common;

public static class AuthErrors
{
    public static Error OtpRateLimitExceeded => Error.Conflict(
        code: "Auth.OtpRateLimit",
        description: "Too many OTP requests. Please wait before trying again.");

    public static Error OtpSendFailed => Error.Failure(
        code: "Auth.OtpSendFailed",
        description: "We could not send the OTP. Please try again shortly.");

    public static Error InvalidOtp => Error.Validation(
        code: "Auth.InvalidOtp",
        description: "Invalid or expired OTP code.");

    public static Error InvalidRefreshToken => Error.Unauthorized(
        code: "Auth.InvalidRefreshToken",
        description: "Invalid or expired refresh token.");

    public static Error RefreshTokenExpired => Error.Unauthorized(
        code: "Auth.RefreshTokenExpired",
        description: "Your session expired. Please sign in again.");

    public static Error StudentNotFound => Error.NotFound(
        code: "Auth.StudentNotFound",
        description: "Student profile could not be located.");

    public static Error PhoneAlreadyRegistered => Error.Conflict(
        code: "Auth.PhoneAlreadyRegistered",
        description: "An account with this phone number already exists.");

    public static Error InvalidCredentials => Error.Unauthorized(
        code: "Auth.InvalidCredentials",
        description: "Invalid phone number or password.");

    public static Error WeakPassword => Error.Validation(
        code: "Auth.WeakPassword",
        description: "Password must be at least 8 characters with a mix of letters and numbers.");
}
