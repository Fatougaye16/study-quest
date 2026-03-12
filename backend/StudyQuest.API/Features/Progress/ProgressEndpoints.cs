using System.Security.Claims;
using MediatR;
using StudyQuest.API.Common;
using StudyQuest.API.Extensions;
using StudyQuest.API.Features.Progress.GetAchievements;
using StudyQuest.API.Features.Progress.GetProgress;
using StudyQuest.API.Features.Progress.GetStreakCalendar;
using StudyQuest.API.Features.Progress.GetWeeklyStudy;

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

        group.MapGet("/weekly", async (ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new GetWeeklyStudyQuery(studentId), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapGet("/streak-calendar", async (ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new GetStreakCalendarQuery(studentId), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        return builder;
    }
}
