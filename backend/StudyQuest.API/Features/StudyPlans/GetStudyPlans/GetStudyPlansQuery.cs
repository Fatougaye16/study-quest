using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.StudyPlans.Common;

namespace StudyQuest.API.Features.StudyPlans.GetStudyPlans;

public record GetStudyPlansQuery(Guid StudentId) : IRequest<ErrorOr<List<StudyPlanResponse>>>;

internal sealed class GetStudyPlansQueryHandler : IRequestHandler<GetStudyPlansQuery, ErrorOr<List<StudyPlanResponse>>>
{
    private readonly AppDbContext _db;

    public GetStudyPlansQueryHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<List<StudyPlanResponse>>> Handle(GetStudyPlansQuery request, CancellationToken ct)
    {
        var plans = await _db.StudyPlans
            .Where(p => p.StudentId == request.StudentId)
            .Include(p => p.Subject)
            .Include(p => p.Items).ThenInclude(i => i.Topic)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(ct);

        return plans.Select(p => p.ToResponse()).ToList();
    }
}
