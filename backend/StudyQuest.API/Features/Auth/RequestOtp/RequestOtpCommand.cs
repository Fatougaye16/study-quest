using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Features.Auth.RequestOtp;

public record RequestOtpCommand(string PhoneNumber) : IRequest<ErrorOr<Unit>>;

internal sealed class RequestOtpCommandHandler : IRequestHandler<RequestOtpCommand, ErrorOr<Unit>>
{
    private readonly AppDbContext _db;
    private readonly IOtpService _otpService;

    public RequestOtpCommandHandler(AppDbContext db, IOtpService otpService)
    {
        _db = db;
        _otpService = otpService;
    }

    public async Task<ErrorOr<Unit>> Handle(RequestOtpCommand request, CancellationToken cancellationToken)
    {
        // Only send OTP if a registered student with OTP enabled exists.
        // Return success regardless to prevent phone number enumeration.
        var studentExists = await _db.Students
            .AnyAsync(s => s.PhoneNumber == request.PhoneNumber && s.IsOtpEnabled, cancellationToken);

        if (!studentExists)
            return Unit.Value;

        var result = await _otpService.GenerateAndSendOtpAsync(request.PhoneNumber, cancellationToken);
        return result.IsError ? result.Errors : Unit.Value;
    }
}
