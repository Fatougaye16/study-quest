using System.Security.Claims;
using MediatR;
using StudyQuest.API.Common;
using StudyQuest.API.Extensions;
using StudyQuest.API.Features.Auth.Common;
using StudyQuest.API.Features.Auth.Login;
using StudyQuest.API.Features.Auth.Logout;
using StudyQuest.API.Features.Auth.RefreshToken;
using StudyQuest.API.Features.Auth.Register;
using StudyQuest.API.Features.Auth.RequestOtp;
using StudyQuest.API.Features.Auth.VerifyOtp;

namespace StudyQuest.API.Features.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/auth");

        group.MapPost("/register",
            async (RegisterRequest req, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(
                    new RegisterCommand(req.PhoneNumber, req.Password, req.FirstName, req.LastName, req.Grade), ct);
                return result.Match(
                    auth => Results.Created("/api/profile", auth),
                    errors => errors.ToProblemResult());
            })
            .AllowAnonymous();

        group.MapPost("/login",
            async (LoginRequest req, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new LoginCommand(req.PhoneNumber, req.Password), ct);
                return result.Match(
                    auth => Results.Ok(auth),
                    errors => errors.ToProblemResult());
            })
            .AllowAnonymous();

        group.MapPost("/request-otp",
            async (RequestOtpCommand command, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);
                return result.Match(
                    _ => Results.Ok(new { message = "OTP sent successfully" }),
                    errors => errors.ToProblemResult());
            })
            .AllowAnonymous();

        group.MapPost("/verify-otp",
            async (VerifyOtpCommand command, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);
                return result.Match(
                    auth => Results.Ok(auth),
                    errors => errors.ToProblemResult());
            })
            .AllowAnonymous();

        group.MapPost("/refresh",
            async (RefreshTokenCommand command, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);
                return result.Match(
                    auth => Results.Ok(auth),
                    errors => errors.ToProblemResult());
            })
            .AllowAnonymous();

        group.MapPost("/logout",
            async (ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
            {
                if (!user.TryGetStudentId(out var studentId))
                {
                    return Results.Unauthorized();
                }

                var result = await sender.Send(new LogoutCommand(studentId), ct);
                return result.Match(
                    _ => Results.Ok(new { message = "Logged out successfully" }),
                    errors => errors.ToProblemResult());
            })
            .RequireAuthorization();

        return builder;
    }
}
