using FluentValidation;

namespace StudyQuest.API.Features.Subjects.CreateNote;

public sealed class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
{
    public CreateNoteCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(300);

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.");
    }
}
