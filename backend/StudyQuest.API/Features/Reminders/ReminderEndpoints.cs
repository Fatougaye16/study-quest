using System.Security.Claims;
using MediatR;
using StudyQuest.API.Common;
using StudyQuest.API.Extensions;
using StudyQuest.API.Features.Reminders.Common;
using StudyQuest.API.Features.Reminders.CreateReminder;
using StudyQuest.API.Features.Reminders.DeleteReminder;
using StudyQuest.API.Features.Reminders.GetReminders;

namespace StudyQuest.API.Features.Reminders;

public static class ReminderEndpoints
{
    public static IEndpointRouteBuilder MapReminderEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/reminders").RequireAuthorization();

        group.MapGet("/", async (ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new GetRemindersQuery(studentId), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapPost("/", async (ClaimsPrincipal user, CreateReminderRequest req, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(
                new CreateReminderCommand(studentId, req.Title, req.Message, req.ScheduledAt, req.Type, req.IsRecurring), ct);
            return result.Match(r => Results.Created($"/api/reminders/{r.Id}", r), errors => errors.ToProblemResult());
        });

        group.MapDelete("/{id:guid}", async (Guid id, ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new DeleteReminderCommand(studentId, id), ct);
            return result.Match(_ => Results.NoContent(), errors => errors.ToProblemResult());
        });

        return builder;
    }
}
