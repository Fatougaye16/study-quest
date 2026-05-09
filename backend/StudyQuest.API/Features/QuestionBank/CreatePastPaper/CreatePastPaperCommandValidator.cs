using FluentValidation;

namespace StudyQuest.API.Features.QuestionBank.CreatePastPaper;

public sealed class CreatePastPaperCommandValidator : AbstractValidator<CreatePastPaperCommand>
{
    public CreatePastPaperCommandValidator()
    {
        RuleFor(x => x.SubjectId).NotEmpty().WithMessage("Subject is required.");

        RuleFor(x => x.Year)
            .GreaterThan(2000).WithMessage("Year must be after 2000.")
            .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("Year cannot be in the future.");

        RuleFor(x => x.ExamType)
            .NotEmpty()
            .Must(t => t is "WASSCE" or "BECE" or "NECO")
            .WithMessage("Exam type must be WASSCE, BECE, or NECO.");

        RuleFor(x => x.PaperNumber).GreaterThan(0).WithMessage("Paper number must be positive.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(300);
    }
}
