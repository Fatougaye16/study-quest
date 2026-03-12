using System.Security.Claims;
using System.Threading.RateLimiting;
using MediatR;
using StudyQuest.API.Common;
using StudyQuest.API.Extensions;
using StudyQuest.API.Features.AI.Common;
using StudyQuest.API.Features.AI.Explain;
using StudyQuest.API.Features.AI.GenerateFlashcards;
using StudyQuest.API.Features.AI.GenerateQuiz;
using StudyQuest.API.Features.AI.GenerateStudyPlan;
using StudyQuest.API.Features.AI.Summarize;

namespace StudyQuest.API.Features.AI;

public static class AIEndpoints
{
    public static IEndpointRouteBuilder MapAIEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/ai")
            .RequireAuthorization()
            .RequireRateLimiting("ai");

        group.MapPost("/summarize", async (ClaimsPrincipal user, SummarizeRequest req, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new SummarizeCommand(studentId, req.TopicId, req.Content, req.Grade), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapPost("/flashcards", async (ClaimsPrincipal user, FlashcardRequest req, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new GenerateFlashcardsCommand(studentId, req.TopicId, req.Content, req.Count), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapPost("/quiz", async (ClaimsPrincipal user, QuizRequest req, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new GenerateQuizCommand(studentId, req.TopicId, req.Difficulty, req.QuestionCount), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapPost("/explain", async (ClaimsPrincipal user, ExplainRequest req, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new ExplainCommand(studentId, req.TopicId, req.SpecificQuestion, req.Grade), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapPost("/study-plan", async (ClaimsPrincipal user, AIStudyPlanRequest req, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new GenerateAIStudyPlanCommand(studentId, req.SubjectId, req.TopicIds, req.DurationDays), ct);
            return result.Match(plan => Results.Ok(plan), errors => errors.ToProblemResult());
        });

        return builder;
    }
}
