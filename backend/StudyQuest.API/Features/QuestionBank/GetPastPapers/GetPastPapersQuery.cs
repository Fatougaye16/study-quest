using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.QuestionBank.Common;

namespace StudyQuest.API.Features.QuestionBank.GetPastPapers;

public record GetPastPapersQuery(Guid? SubjectId, int? Year, string? ExamType)
    : IRequest<ErrorOr<List<PastPaperResponse>>>;

internal sealed class GetPastPapersQueryHandler : IRequestHandler<GetPastPapersQuery, ErrorOr<List<PastPaperResponse>>>
{
    private readonly AppDbContext _db;

    public GetPastPapersQueryHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<List<PastPaperResponse>>> Handle(GetPastPapersQuery request, CancellationToken ct)
    {
        var query = _db.PastPapers
            .Include(p => p.Subject)
            .Include(p => p.Questions)
            .AsQueryable();

        if (request.SubjectId.HasValue)
            query = query.Where(p => p.SubjectId == request.SubjectId.Value);

        if (request.Year.HasValue)
            query = query.Where(p => p.Year == request.Year.Value);

        if (!string.IsNullOrWhiteSpace(request.ExamType) &&
            Enum.TryParse<Models.ExamType>(request.ExamType, true, out var examType))
            query = query.Where(p => p.ExamType == examType);

        var papers = await query.OrderByDescending(p => p.Year).ThenBy(p => p.PaperNumber).ToListAsync(ct);

        return papers.Select(p => new PastPaperResponse(
            p.Id, p.SubjectId, p.Subject.Name, p.Year,
            p.ExamType.ToString(), p.PaperNumber, p.Title,
            p.Questions.Count, p.CreatedAt
        )).ToList();
    }
}
