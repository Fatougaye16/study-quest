using System.Security.Claims;
using MediatR;
using StudyQuest.API.Common;
using StudyQuest.API.Extensions;
using StudyQuest.API.Features.Enrollments.Common;
using StudyQuest.API.Features.Enrollments.Enroll;
using StudyQuest.API.Features.Enrollments.GetEnrollments;
using StudyQuest.API.Features.Enrollments.Unenroll;

namespace StudyQuest.API.Features.Enrollments;

public static class EnrollmentEndpoints
{
    public static IEndpointRouteBuilder MapEnrollmentEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/enrollments").RequireAuthorization();

        group.MapGet("/", async (ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new GetEnrollmentsQuery(studentId), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapPost("/", async (ClaimsPrincipal user, CreateEnrollmentRequest request, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new EnrollCommand(studentId, request.SubjectId), ct);
            return result.Match(e => Results.Created($"/api/enrollments", e), errors => errors.ToProblemResult());
        });

        group.MapDelete("/{enrollmentId:guid}", async (Guid enrollmentId, ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new UnenrollCommand(studentId, enrollmentId), ct);
            return result.Match(_ => Results.NoContent(), errors => errors.ToProblemResult());
        });

        return builder;
    }
}
