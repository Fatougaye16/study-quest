using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.StudyPlans.Common;

namespace StudyQuest.API.Features.StudyPlans.DeleteStudyPlan;

public record DeleteStudyPlanCommand(Guid StudentId, Guid PlanId) : IRequest<ErrorOr<Unit>>;

internal sealed class DeleteStudyPlanCommandHandler : IRequestHandler<DeleteStudyPlanCommand, ErrorOr<Unit>>
{
    private readonly AppDbContext _db;

    public DeleteStudyPlanCommandHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<Unit>> Handle(DeleteStudyPlanCommand request, CancellationToken ct)
    {
        var plan = await _db.StudyPlans
            .FirstOrDefaultAsync(p => p.Id == request.PlanId && p.StudentId == request.StudentId, ct);

        if (plan is null)
            return StudyPlanErrors.PlanNotFound;

        _db.StudyPlans.Remove(plan);
        await _db.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
