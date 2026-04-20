using FluentValidation;

namespace StudyQuest.API.Features.Timetable.CreateEntry;

public sealed class CreateTimetableEntryCommandValidator : AbstractValidator<CreateTimetableEntryCommand>
{
    public CreateTimetableEntryCommandValidator()
    {
        RuleFor(x => x.SubjectId).NotEmpty().WithMessage("Subject is required.");

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.");

        RuleFor(x => x.Location)
            .MaximumLength(200)
            .When(x => x.Location is not null);
    }
}
