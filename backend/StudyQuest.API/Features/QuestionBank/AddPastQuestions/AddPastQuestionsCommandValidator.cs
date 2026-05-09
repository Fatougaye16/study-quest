using FluentValidation;
using StudyQuest.API.Features.QuestionBank.Common;

namespace StudyQuest.API.Features.QuestionBank.AddPastQuestions;

public sealed class AddPastQuestionsCommandValidator : AbstractValidator<AddPastQuestionsCommand>
{
    public AddPastQuestionsCommandValidator()
    {
        RuleFor(x => x.PastPaperId).NotEmpty().WithMessage("Past paper ID is required.");

        RuleFor(x => x.Questions)
            .NotEmpty().WithMessage("At least one question is required.");

        RuleForEach(x => x.Questions).ChildRules(q =>
        {
            q.RuleFor(x => x.QuestionNumber).GreaterThan(0).WithMessage("Question number must be positive.");
            q.RuleFor(x => x.QuestionText).NotEmpty().WithMessage("Question text is required.");
            q.RuleFor(x => x.Difficulty).InclusiveBetween(1, 3).WithMessage("Difficulty must be between 1 and 3.");
        });
    }
}
