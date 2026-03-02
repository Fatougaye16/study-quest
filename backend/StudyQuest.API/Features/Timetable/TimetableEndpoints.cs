using System.Security.Claims;
using MediatR;
using StudyQuest.API.Common;
using StudyQuest.API.Extensions;
using StudyQuest.API.Features.Timetable.Common;
using StudyQuest.API.Features.Timetable.CreateEntry;
using StudyQuest.API.Features.Timetable.DeleteEntry;
using StudyQuest.API.Features.Timetable.GetTimetable;
using StudyQuest.API.Features.Timetable.UpdateEntry;

namespace StudyQuest.API.Features.Timetable;

public static class TimetableEndpoints
{
    public static IEndpointRouteBuilder MapTimetableEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/timetable").RequireAuthorization();

        group.MapGet("/", async (ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new GetTimetableQuery(studentId), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapPost("/", async (ClaimsPrincipal user, CreateTimetableEntryRequest req, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new CreateTimetableEntryCommand(
                studentId, req.SubjectId, req.DayOfWeek, req.StartTime, req.EndTime, req.Location), ct);
            return result.Match(e => Results.Created("/api/timetable", e), errors => errors.ToProblemResult());
        });

        group.MapPut("/{entryId:guid}", async (Guid entryId, ClaimsPrincipal user, UpdateTimetableEntryRequest req, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new UpdateTimetableEntryCommand(
                studentId, entryId, req.SubjectId, req.DayOfWeek, req.StartTime, req.EndTime, req.Location), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapDelete("/{entryId:guid}", async (Guid entryId, ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new DeleteTimetableEntryCommand(studentId, entryId), ct);
            return result.Match(_ => Results.NoContent(), errors => errors.ToProblemResult());
        });

        return builder;
    }
}
