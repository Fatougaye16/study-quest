using FluentValidation;

namespace StudyQuest.API.Features.StudyPlans.CreateStudyPlan;

public sealed class CreateStudyPlanCommandValidator : AbstractValidator<CreateStudyPlanCommand>
{
    public CreateStudyPlanCommandValidator()
    {
        RuleFor(x => x.SubjectId).NotEmpty().WithMessage("Subject is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one study plan item is required.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.TopicId).NotEmpty().WithMessage("Topic is required for each item.");
            item.RuleFor(i => i.DurationMinutes).GreaterThan(0).WithMessage("Duration must be greater than zero.");
        });
    }
}
