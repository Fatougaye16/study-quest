using FluentValidation;

namespace StudyQuest.API.Features.Auth.RequestOtp;

public sealed class RequestOtpCommandValidator : AbstractValidator<RequestOtpCommand>
{
    public RequestOtpCommandValidator()
    {
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required.");
    }
}
