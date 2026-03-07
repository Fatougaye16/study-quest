using ErrorOr;

namespace StudyQuest.API.Services.Interfaces;

public interface IOtpService
{
    /// <summary>
    /// Generates a 6-digit OTP, stores its hash in cache, and sends it via SMS (or logs it in Development).
    /// Returns an error if rate-limited or if sending fails.
    /// </summary>
    Task<ErrorOr<Success>> GenerateAndSendOtpAsync(string phoneNumber, CancellationToken ct = default);

    /// <summary>
    /// Verifies the submitted OTP code against the stored hash.
    /// On success, removes the OTP from cache. Returns an error if invalid or expired.
    /// </summary>
    ErrorOr<Success> VerifyOtp(string phoneNumber, string otpCode);
}
