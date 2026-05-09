using FluentValidation;

namespace StudyQuest.API.Features.StudySessions.CreateSession;

public sealed class CreateStudySessionCommandValidator : AbstractValidator<CreateStudySessionCommand>
{
    public CreateStudySessionCommandValidator()
    {
        RuleFor(x => x.SubjectId).NotEmpty().WithMessage("Subject is required.");
        RuleFor(x => x.DurationMinutes).GreaterThan(0).WithMessage("Duration must be greater than zero.");
    }
}
