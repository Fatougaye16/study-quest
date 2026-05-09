using FluentValidation;

namespace StudyQuest.API.Features.Auth.Common;

public static class AuthValidationRules
{
    // E.164-ish validation that accepts an optional leading '+', 8-15 digits total.
    private const string PhoneRegex = @"^\+?[1-9]\d{7,14}$";

    public static IRuleBuilderOptions<T, string> ValidPhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(20)
            .Matches(PhoneRegex).WithMessage("Phone number must be in a valid international format.");
    }

    public static IRuleBuilderOptions<T, string> StrongPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .MaximumLength(128).WithMessage("Password cannot exceed 128 characters.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one digit.");
    }
}