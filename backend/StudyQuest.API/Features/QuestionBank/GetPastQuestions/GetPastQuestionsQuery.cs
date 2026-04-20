using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.QuestionBank.Common;

namespace StudyQuest.API.Features.QuestionBank.GetPastQuestions;

public record GetPastQuestionsQuery(Guid PastPaperId, Guid? TopicId, int? Difficulty)
    : IRequest<ErrorOr<List<PastQuestionResponse>>>;

internal sealed class GetPastQuestionsQueryHandler
    : IRequestHandler<GetPastQuestionsQuery, ErrorOr<List<PastQuestionResponse>>>
{
    private readonly AppDbContext _db;

    public GetPastQuestionsQueryHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<List<PastQuestionResponse>>> Handle(GetPastQuestionsQuery request, CancellationToken ct)
    {
        var paperExists = await _db.PastPapers.AnyAsync(p => p.Id == request.PastPaperId, ct);
        if (!paperExists) return QuestionBankErrors.PaperNotFound;

        var query = _db.PastQuestions
            .Where(q => q.PastPaperId == request.PastPaperId)
            .Include(q => q.Topic)
            .AsQueryable();

        if (request.TopicId.HasValue)
            query = query.Where(q => q.TopicId == request.TopicId.Value);

        if (request.Difficulty.HasValue)
            query = query.Where(q => q.Difficulty == request.Difficulty.Value);

        var questions = await query.OrderBy(q => q.QuestionNumber).ToListAsync(ct);

        return questions.Select(q => new PastQuestionResponse(
            q.Id, q.QuestionNumber, q.QuestionText, q.AnswerText,
            q.Marks, q.ImageUrl, q.Difficulty, q.Topic?.Name
        )).ToList();
    }
}
