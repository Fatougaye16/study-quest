using FluentValidation;
using StudyQuest.API.Features.Auth.Login;
using StudyQuest.API.Features.Auth.Common;

namespace StudyQuest.API.Features.Auth.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.PhoneNumber).ValidPhoneNumber();
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
    }
}
