using FluentValidation;
using StudyQuest.API.Features.Auth.Login;

namespace StudyQuest.API.Features.Auth.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
    }
}
