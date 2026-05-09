using System.Security.Claims;
using MediatR;
using StudyQuest.API.Common;
using StudyQuest.API.Extensions;
using StudyQuest.API.Features.QuestionBank.AddPastQuestions;
using StudyQuest.API.Features.QuestionBank.Common;
using StudyQuest.API.Features.QuestionBank.CreatePastPaper;
using StudyQuest.API.Features.QuestionBank.DeletePastPaper;
using StudyQuest.API.Features.QuestionBank.GetPastPapers;
using StudyQuest.API.Features.QuestionBank.GetPastQuestions;

namespace StudyQuest.API.Features.QuestionBank;

public static class QuestionBankEndpoints
{
    public static IEndpointRouteBuilder MapQuestionBankEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/question-bank").RequireAuthorization();

        group.MapGet("/papers", async (Guid? subjectId, int? year, string? examType, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetPastPapersQuery(subjectId, year, examType), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapGet("/papers/{paperId:guid}/questions", async (Guid paperId, Guid? topicId, int? difficulty, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetPastQuestionsQuery(paperId, topicId, difficulty), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapPost("/papers", async (ClaimsPrincipal user, CreatePastPaperRequest req, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new CreatePastPaperCommand(studentId, req.SubjectId, req.Year, req.ExamType, req.PaperNumber, req.Title), ct);
            return result.Match(paper => Results.Created($"/api/question-bank/papers/{paper.Id}", paper), errors => errors.ToProblemResult());
        });

        group.MapPost("/papers/{paperId:guid}/questions", async (Guid paperId, ClaimsPrincipal user, AddPastQuestionsRequest req, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new AddPastQuestionsCommand(studentId, paperId, req.Questions), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapDelete("/papers/{paperId:guid}", async (Guid paperId, ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            if (!user.TryGetStudentId(out var studentId)) return Results.Unauthorized();
            var result = await sender.Send(new DeletePastPaperCommand(studentId, paperId), ct);
            return result.Match(_ => Results.NoContent(), errors => errors.ToProblemResult());
        });

        return builder;
    }
}
