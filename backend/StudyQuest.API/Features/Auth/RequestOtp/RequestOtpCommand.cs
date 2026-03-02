using System.Security.Cryptography;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using StudyQuest.API.Configuration;
using StudyQuest.API.Features.Auth.Common;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace StudyQuest.API.Features.Auth.RequestOtp;

public record RequestOtpCommand(string PhoneNumber) : IRequest<ErrorOr<Unit>>;

internal sealed class RequestOtpCommandHandler : IRequestHandler<RequestOtpCommand, ErrorOr<Unit>>
{
    private readonly IMemoryCache _cache;
    private readonly IWebHostEnvironment _environment;
    private readonly TwilioSettings _twilioSettings;
    private readonly ILogger<RequestOtpCommandHandler> _logger;

    public RequestOtpCommandHandler(
        IMemoryCache cache,
        IWebHostEnvironment environment,
        IOptions<TwilioSettings> twilioOptions,
        ILogger<RequestOtpCommandHandler> logger)
    {
        _cache = cache;
        _environment = environment;
        _twilioSettings = twilioOptions.Value;
        _logger = logger;
    }

    public async Task<ErrorOr<Unit>> Handle(RequestOtpCommand request, CancellationToken cancellationToken)
    {
        var attempts = _cache.GetOrCreate(BuildRateLimitKey(request.PhoneNumber), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return 0;
        });

        if (attempts >= 3)
        {
            return AuthErrors.OtpRateLimitExceeded;
        }

        _cache.Set(BuildRateLimitKey(request.PhoneNumber), attempts + 1, TimeSpan.FromMinutes(10));

        var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
        var cacheKey = OtpHelper.BuildCacheKey(request.PhoneNumber);
        _cache.Set(cacheKey, OtpHelper.Hash(otp), TimeSpan.FromMinutes(5));

        if (_environment.IsDevelopment())
        {
            _logger.LogInformation("DEV OTP for {PhoneNumber}: {Otp}", request.PhoneNumber, otp);
            return Unit.Value;
        }

        try
        {
            TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);
            await MessageResource.CreateAsync(
                to: new PhoneNumber(request.PhoneNumber),
                from: new PhoneNumber(_twilioSettings.PhoneNumber),
                body: $"Your Study Quest verification code is: {otp}. It expires in 5 minutes.");

            return Unit.Value;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to send OTP to {PhoneNumber}", request.PhoneNumber);
            return AuthErrors.OtpSendFailed;
        }
    }

    private static string BuildRateLimitKey(string phoneNumber) => $"otp_rate_{phoneNumber}";
}
