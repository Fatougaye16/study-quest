using FluentValidation;
using StudyQuest.API.Features.Auth.Common;

namespace StudyQuest.API.Features.Auth.RequestOtp;

public sealed class RequestOtpCommandValidator : AbstractValidator<RequestOtpCommand>
{
    public RequestOtpCommandValidator()
    {
        RuleFor(x => x.PhoneNumber).ValidPhoneNumber();
    }
}
