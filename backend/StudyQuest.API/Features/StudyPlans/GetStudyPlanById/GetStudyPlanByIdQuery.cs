using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.StudyPlans.Common;

namespace StudyQuest.API.Features.StudyPlans.GetStudyPlanById;

public record GetStudyPlanByIdQuery(Guid StudentId, Guid PlanId) : IRequest<ErrorOr<StudyPlanResponse>>;

internal sealed class GetStudyPlanByIdQueryHandler : IRequestHandler<GetStudyPlanByIdQuery, ErrorOr<StudyPlanResponse>>
{
    private readonly AppDbContext _db;

    public GetStudyPlanByIdQueryHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<StudyPlanResponse>> Handle(GetStudyPlanByIdQuery request, CancellationToken ct)
    {
        var plan = await _db.StudyPlans
            .Where(p => p.Id == request.PlanId && p.StudentId == request.StudentId)
            .Include(p => p.Subject)
            .Include(p => p.Items).ThenInclude(i => i.Topic)
            .FirstOrDefaultAsync(ct);

        if (plan is null)
            return StudyPlanErrors.PlanNotFound;

        return plan.ToResponse();
    }
}
