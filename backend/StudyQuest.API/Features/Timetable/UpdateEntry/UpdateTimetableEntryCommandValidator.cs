using FluentValidation;

namespace StudyQuest.API.Features.Timetable.UpdateEntry;

public sealed class UpdateTimetableEntryCommandValidator : AbstractValidator<UpdateTimetableEntryCommand>
{
    public UpdateTimetableEntryCommandValidator()
    {
        RuleFor(x => x.EntryId).NotEmpty().WithMessage("Entry ID is required.");
        RuleFor(x => x.SubjectId).NotEmpty().WithMessage("Subject is required.");

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.");

        RuleFor(x => x.Location)
            .MaximumLength(200)
            .When(x => x.Location is not null);
    }
}
