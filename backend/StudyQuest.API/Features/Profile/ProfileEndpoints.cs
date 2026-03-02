using System.Security.Claims;
using MediatR;
using StudyQuest.API.Common;
using StudyQuest.API.Extensions;
using StudyQuest.API.Features.Auth.Common;
using StudyQuest.API.Features.Profile.GetProfile;
using StudyQuest.API.Features.Profile.UpdateProfile;

namespace StudyQuest.API.Features.Profile;

public static class ProfileEndpoints
{
    public static IEndpointRouteBuilder MapProfileEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/profile").RequireAuthorization();

        group.MapGet("/", async (ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId))
            {
                return Results.Unauthorized();
            }

            var result = await sender.Send(new GetProfileQuery(studentId), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapPut("/", async (ClaimsPrincipal user, UpdateProfileRequest request, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId))
            {
                return Results.Unauthorized();
            }

            var command = new UpdateProfileCommand(
                StudentId: studentId,
                FirstName: request.FirstName,
                LastName: request.LastName,
                Grade: request.Grade,
                DailyGoalMinutes: request.DailyGoalMinutes);

            var result = await sender.Send(command, ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        return builder;
    }
}
