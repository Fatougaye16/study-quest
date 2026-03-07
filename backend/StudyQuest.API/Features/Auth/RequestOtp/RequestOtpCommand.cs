using ErrorOr;
using MediatR;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Features.Auth.RequestOtp;

public record RequestOtpCommand(string PhoneNumber) : IRequest<ErrorOr<Unit>>;

internal sealed class RequestOtpCommandHandler : IRequestHandler<RequestOtpCommand, ErrorOr<Unit>>
{
    private readonly IOtpService _otpService;

    public RequestOtpCommandHandler(IOtpService otpService) => _otpService = otpService;

    public async Task<ErrorOr<Unit>> Handle(RequestOtpCommand request, CancellationToken cancellationToken)
    {
        var result = await _otpService.GenerateAndSendOtpAsync(request.PhoneNumber, cancellationToken);
        return result.IsError ? result.Errors : Unit.Value;
    }
}
