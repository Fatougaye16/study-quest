using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Subjects.Common;

namespace StudyQuest.API.Features.Subjects.GetQuestions;

public record GetQuestionsQuery(Guid TopicId) : IRequest<ErrorOr<List<QuestionResponse>>>;

internal sealed class GetQuestionsQueryHandler : IRequestHandler<GetQuestionsQuery, ErrorOr<List<QuestionResponse>>>
{
    private readonly AppDbContext _db;

    public GetQuestionsQueryHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<List<QuestionResponse>>> Handle(GetQuestionsQuery request, CancellationToken ct)
    {
        var questions = await _db.Questions
            .Where(q => q.TopicId == request.TopicId)
            .OrderBy(q => q.Difficulty)
            .Select(q => new QuestionResponse(q.Id, q.TopicId, q.QuestionText, q.AnswerText, q.Difficulty, q.IsAIGenerated))
            .ToListAsync(ct);

        return questions;
    }
}
