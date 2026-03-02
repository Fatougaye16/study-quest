using System.Security.Claims;
using MediatR;
using StudyQuest.API.Common;
using StudyQuest.API.Extensions;
using StudyQuest.API.Features.StudySessions.Common;
using StudyQuest.API.Features.StudySessions.CreateSession;
using StudyQuest.API.Features.StudySessions.GetSessions;

namespace StudyQuest.API.Features.StudySessions;

public static class StudySessionEndpoints
{
    public static IEndpointRouteBuilder MapStudySessionEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/study-sessions").RequireAuthorization();

        group.MapPost("/", async (ClaimsPrincipal user, CreateStudySessionRequest req, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new CreateStudySessionCommand(
                studentId, req.SubjectId, req.TopicId, req.StartedAt, req.EndedAt, req.DurationMinutes, req.Notes), ct);
            return result.Match(s => Results.Created("/api/study-sessions", s), errors => errors.ToProblemResult());
        });

        group.MapGet("/", async (ClaimsPrincipal user, ISender sender, CancellationToken ct,
            Guid? subjectId = null, DateTime? from = null, DateTime? to = null) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new GetSessionsQuery(studentId, subjectId, from, to), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        return builder;
    }
}
