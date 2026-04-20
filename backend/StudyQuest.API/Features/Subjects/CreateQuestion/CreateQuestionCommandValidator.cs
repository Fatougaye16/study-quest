using FluentValidation;

namespace StudyQuest.API.Features.Subjects.CreateQuestion;

public sealed class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionCommandValidator()
    {
        RuleFor(x => x.QuestionText).NotEmpty().WithMessage("Question text is required.");
        RuleFor(x => x.AnswerText).NotEmpty().WithMessage("Answer text is required.");
        RuleFor(x => x.Difficulty).InclusiveBetween(1, 3).WithMessage("Difficulty must be between 1 (Easy) and 3 (Hard).");
    }
}
