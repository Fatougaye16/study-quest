using ErrorOr;
using MediatR;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Subjects.Common;
using StudyQuest.API.Models;

namespace StudyQuest.API.Features.Subjects.CreateQuestion;

public record CreateQuestionCommand(Guid TopicId, string QuestionText, string AnswerText, int Difficulty) : IRequest<ErrorOr<QuestionResponse>>;

internal sealed class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, ErrorOr<QuestionResponse>>
{
    private readonly AppDbContext _db;

    public CreateQuestionCommandHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<QuestionResponse>> Handle(CreateQuestionCommand request, CancellationToken ct)
    {
        var topic = await _db.Topics.FindAsync([request.TopicId], ct);
        if (topic is null)
            return SubjectErrors.TopicNotFound;

        var question = new Question
        {
            Id = Guid.NewGuid(),
            TopicId = request.TopicId,
            QuestionText = request.QuestionText,
            AnswerText = request.AnswerText,
            Difficulty = request.Difficulty,
            IsAIGenerated = false
        };

        _db.Questions.Add(question);
        await _db.SaveChangesAsync(ct);

        return new QuestionResponse(question.Id, question.TopicId, question.QuestionText, question.AnswerText, question.Difficulty, question.IsAIGenerated);
    }
}
