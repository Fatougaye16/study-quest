using FluentValidation;
using StudyQuest.API.Features.Auth.Common;

namespace StudyQuest.API.Features.Auth.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.PhoneNumber).ValidPhoneNumber();

        RuleFor(x => x.Password).StrongPassword();

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Grade)
            .InclusiveBetween(10, 12).WithMessage("Grade must be between 10 and 12.");
    }
}
