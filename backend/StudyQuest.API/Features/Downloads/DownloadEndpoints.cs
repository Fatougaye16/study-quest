using System.Security.Claims;
using MediatR;
using StudyQuest.API.Common;
using StudyQuest.API.Extensions;
using StudyQuest.API.Features.Downloads.DownloadFlashcards;
using StudyQuest.API.Features.Downloads.DownloadNotes;
using StudyQuest.API.Features.Downloads.DownloadPastPaper;
using StudyQuest.API.Features.Downloads.DownloadStudyPlan;

namespace StudyQuest.API.Features.Downloads;

public static class DownloadEndpoints
{
    public static IEndpointRouteBuilder MapDownloadEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/downloads").RequireAuthorization();

        group.MapGet("/past-paper/{paperId:guid}", async (Guid paperId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new DownloadPastPaperQuery(paperId), ct);
            return result.Match(
                dl => Results.File(dl.PdfData, "application/pdf", dl.FileName),
                errors => errors.ToProblemResult());
        });

        group.MapGet("/notes/{topicId:guid}", async (Guid topicId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new DownloadNotesQuery(topicId), ct);
            return result.Match(
                dl => Results.File(dl.PdfData, "application/pdf", dl.FileName),
                errors => errors.ToProblemResult());
        });

        group.MapGet("/flashcards/{topicId:guid}", async (Guid topicId, ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new DownloadFlashcardsQuery(topicId, studentId), ct);
            return result.Match(
                dl => Results.File(dl.PdfData, "application/pdf", dl.FileName),
                errors => errors.ToProblemResult());
        });

        group.MapGet("/study-plan/{planId:guid}", async (Guid planId, ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new DownloadStudyPlanQuery(planId, studentId), ct);
            return result.Match(
                dl => Results.File(dl.PdfData, "application/pdf", dl.FileName),
                errors => errors.ToProblemResult());
        });

        return builder;
    }
}
