using System.Security.Claims;
using MediatR;
using StudyQuest.API.Common;
using StudyQuest.API.Extensions;
using StudyQuest.API.Features.Progress.GetAchievements;
using StudyQuest.API.Features.Progress.GetProgress;

namespace StudyQuest.API.Features.Progress;

public static class ProgressEndpoints
{
    public static IEndpointRouteBuilder MapProgressEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/progress").RequireAuthorization();

        group.MapGet("/", async (ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new GetProgressQuery(studentId), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapGet("/achievements", async (ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new GetAchievementsQuery(studentId), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        return builder;
    }
}
