using FluentValidation;
using StudyQuest.API.Features.Profile.UpdateProfile;

namespace StudyQuest.API.Features.Profile.UpdateProfile;

public sealed class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .MaximumLength(100)
            .When(x => x.FirstName is not null);

        RuleFor(x => x.LastName)
            .MaximumLength(100)
            .When(x => x.LastName is not null);

        RuleFor(x => x.Grade)
            .InclusiveBetween(10, 12).WithMessage("Grade must be between 10 and 12.")
            .When(x => x.Grade.HasValue);

        RuleFor(x => x.DailyGoalMinutes)
            .GreaterThan(0).WithMessage("Daily goal must be greater than zero minutes.")
            .When(x => x.DailyGoalMinutes.HasValue);
    }
}
