using ErrorOr;
using MediatR;
using StudyQuest.API.Data;
using StudyQuest.API.Features.StudyPlans.Common;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Features.StudyPlans.CreateStudyPlan;

public record CreateStudyPlanCommand(
    Guid StudentId, Guid SubjectId, string Title,
    DateTime StartDate, DateTime EndDate,
    List<CreateStudyPlanItemRequest> Items) : IRequest<ErrorOr<StudyPlanResponse>>;

internal sealed class CreateStudyPlanCommandHandler : IRequestHandler<CreateStudyPlanCommand, ErrorOr<StudyPlanResponse>>
{
    private readonly AppDbContext _db;
    private readonly IProgressService _progressService;
    private readonly ISender _sender;

    public CreateStudyPlanCommandHandler(AppDbContext db, IProgressService progressService, ISender sender)
    {
        _db = db;
        _progressService = progressService;
        _sender = sender;
    }

    public async Task<ErrorOr<StudyPlanResponse>> Handle(CreateStudyPlanCommand request, CancellationToken ct)
    {
        var subject = await _db.Subjects.FindAsync([request.SubjectId], ct);
        if (subject is null)
            return StudyPlanErrors.SubjectNotFound;

        var plan = new StudyPlan
        {
            Id = Guid.NewGuid(),
            StudentId = request.StudentId,
            SubjectId = request.SubjectId,
            Title = request.Title,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsAIGenerated = false
        };

        _db.StudyPlans.Add(plan);

        foreach (var itemDto in request.Items)
        {
            var topic = await _db.Topics.FindAsync([itemDto.TopicId], ct);
            if (topic is null)
                return StudyPlanErrors.TopicNotFound(itemDto.TopicId);

            _db.StudyPlanItems.Add(new StudyPlanItem
            {
                Id = Guid.NewGuid(),
                StudyPlanId = plan.Id,
                TopicId = itemDto.TopicId,
                ScheduledDate = itemDto.ScheduledDate,
                DurationMinutes = itemDto.DurationMinutes,
                IsCompleted = false
            });
        }

        await _db.SaveChangesAsync(ct);

        await _progressService.AddXPAsync(request.StudentId, request.SubjectId, 30);

        // Reload with navigation properties
        var result = await _sender.Send(new GetStudyPlanById.GetStudyPlanByIdQuery(request.StudentId, plan.Id), ct);
        return result;
    }
}
