using System.Security.Cryptography;
using System.Text;
using ErrorOr;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using StudyQuest.API.Configuration;
using StudyQuest.API.Features.Auth.Common;
using StudyQuest.API.Services.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace StudyQuest.API.Services.Implementations;

public class OtpService : IOtpService
{
    private readonly IMemoryCache _cache;
    private readonly IWebHostEnvironment _environment;
    private readonly TwilioSettings _twilioSettings;
    private readonly ILogger<OtpService> _logger;

    public OtpService(
        IMemoryCache cache,
        IWebHostEnvironment environment,
        IOptions<TwilioSettings> twilioOptions,
        ILogger<OtpService> logger)
    {
        _cache = cache;
        _environment = environment;
        _twilioSettings = twilioOptions.Value;
        _logger = logger;
    }

    public async Task<ErrorOr<Success>> GenerateAndSendOtpAsync(string phoneNumber, CancellationToken ct = default)
    {
        // Rate limiting: max 3 requests per phone per 10 minutes
        var rateLimitKey = BuildRateLimitKey(phoneNumber);
        var attempts = _cache.GetOrCreate(rateLimitKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return 0;
        });

        if (attempts >= 3)
            return AuthErrors.OtpRateLimitExceeded;

        _cache.Set(rateLimitKey, attempts + 1, TimeSpan.FromMinutes(10));

        // Generate 6-digit OTP and store its hash
        var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
        var cacheKey = OtpHelper.BuildCacheKey(phoneNumber);
        _cache.Set(cacheKey, OtpHelper.Hash(otp), TimeSpan.FromMinutes(5));

        // In Development, log the OTP instead of sending SMS
        if (_environment.IsDevelopment())
        {
            _logger.LogInformation("DEV OTP generated for {PhoneNumber}", MaskPhoneNumber(phoneNumber));
            return Result.Success;
        }

        if (string.IsNullOrWhiteSpace(_twilioSettings.AccountSid) ||
            string.IsNullOrWhiteSpace(_twilioSettings.AuthToken) ||
            string.IsNullOrWhiteSpace(_twilioSettings.PhoneNumber))
        {
            _logger.LogError("Twilio settings are not configured. OTP delivery is unavailable.");
            return AuthErrors.OtpSendFailed;
        }

        try
        {
            TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);
            await MessageResource.CreateAsync(
                to: new PhoneNumber(phoneNumber),
                from: new PhoneNumber(_twilioSettings.PhoneNumber),
                body: $"Your Study Quest verification code is: {otp}. It expires in 5 minutes.");

            return Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send OTP to {PhoneNumber}", phoneNumber);
            return AuthErrors.OtpSendFailed;
        }
    }

    public ErrorOr<Success> VerifyOtp(string phoneNumber, string otpCode)
    {
        var cacheKey = OtpHelper.BuildCacheKey(phoneNumber);

        if (!_cache.TryGetValue(cacheKey, out string? storedHash))
            return AuthErrors.InvalidOtp;

        var storedHashBytes = Encoding.UTF8.GetBytes(storedHash);
        var candidateHashBytes = Encoding.UTF8.GetBytes(OtpHelper.Hash(otpCode));

        if (!CryptographicOperations.FixedTimeEquals(storedHashBytes, candidateHashBytes))
            return AuthErrors.InvalidOtp;

        _cache.Remove(cacheKey);
        return Result.Success;
    }

    private static string BuildRateLimitKey(string phoneNumber) => $"otp_rate_{phoneNumber}";

    private static string MaskPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length <= 4)
            return "****";

        return string.Concat(new string('*', phoneNumber.Length - 4), phoneNumber[^4..]);
    }
}
