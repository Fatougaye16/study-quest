using System.Security.Claims;
using MediatR;
using StudyQuest.API.Common;
using StudyQuest.API.Extensions;
using StudyQuest.API.Features.StudyPlans.Common;
using StudyQuest.API.Features.StudyPlans.CreateStudyPlan;
using StudyQuest.API.Features.StudyPlans.DeleteStudyPlan;
using StudyQuest.API.Features.StudyPlans.GetStudyPlanById;
using StudyQuest.API.Features.StudyPlans.GetStudyPlans;
using StudyQuest.API.Features.StudyPlans.ToggleItem;

namespace StudyQuest.API.Features.StudyPlans;

public static class StudyPlanEndpoints
{
    public static IEndpointRouteBuilder MapStudyPlanEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/study-plans").RequireAuthorization();

        group.MapGet("/", async (ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new GetStudyPlansQuery(studentId), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapGet("/{planId:guid}", async (Guid planId, ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new GetStudyPlanByIdQuery(studentId, planId), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapPost("/", async (ClaimsPrincipal user, CreateStudyPlanRequest request, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new CreateStudyPlanCommand(
                studentId, request.SubjectId, request.Title, request.StartDate, request.EndDate, request.Items), ct);
            return result.Match(p => Results.Created($"/api/study-plans/{p.Id}", p), errors => errors.ToProblemResult());
        });

        group.MapPut("/{planId:guid}/items/{itemId:guid}", async (Guid planId, Guid itemId, ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new ToggleStudyPlanItemCommand(studentId, planId, itemId), ct);
            return result.Match(_ => Results.Ok(new { message = "Item toggled" }), errors => errors.ToProblemResult());
        });

        group.MapDelete("/{planId:guid}", async (Guid planId, ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new DeleteStudyPlanCommand(studentId, planId), ct);
            return result.Match(_ => Results.NoContent(), errors => errors.ToProblemResult());
        });

        return builder;
    }
}
