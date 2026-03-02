using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.StudyPlans.Common;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Features.StudyPlans.ToggleItem;

public record ToggleStudyPlanItemCommand(Guid StudentId, Guid PlanId, Guid ItemId) : IRequest<ErrorOr<Unit>>;

internal sealed class ToggleStudyPlanItemCommandHandler : IRequestHandler<ToggleStudyPlanItemCommand, ErrorOr<Unit>>
{
    private readonly AppDbContext _db;
    private readonly IProgressService _progressService;

    public ToggleStudyPlanItemCommandHandler(AppDbContext db, IProgressService progressService)
    {
        _db = db;
        _progressService = progressService;
    }

    public async Task<ErrorOr<Unit>> Handle(ToggleStudyPlanItemCommand request, CancellationToken ct)
    {
        var plan = await _db.StudyPlans
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == request.PlanId && p.StudentId == request.StudentId, ct);

        if (plan is null)
            return StudyPlanErrors.PlanNotFound;

        var item = plan.Items.FirstOrDefault(i => i.Id == request.ItemId);
        if (item is null)
            return StudyPlanErrors.ItemNotFound;

        item.IsCompleted = !item.IsCompleted;
        item.CompletedAt = item.IsCompleted ? DateTime.UtcNow : null;

        await _db.SaveChangesAsync(ct);

        if (item.IsCompleted)
        {
            var xp = Math.Max(10, item.DurationMinutes / 10 * 10);
            await _progressService.AddXPAsync(request.StudentId, plan.SubjectId, xp);
            await _progressService.CheckAndUnlockAchievementsAsync(request.StudentId);
        }

        return Unit.Value;
    }
}
