using System.Security.Claims;
using MediatR;
using StudyQuest.API.Common;
using StudyQuest.API.Extensions;
using StudyQuest.API.Features.Subjects.Common;
using StudyQuest.API.Features.Subjects.CreateNote;
using StudyQuest.API.Features.Subjects.CreateQuestion;
using StudyQuest.API.Features.Subjects.GetNotes;
using StudyQuest.API.Features.Subjects.GetQuestions;
using StudyQuest.API.Features.Subjects.GetSubjects;
using StudyQuest.API.Features.Subjects.GetTopics;

namespace StudyQuest.API.Features.Subjects;

public static class SubjectEndpoints
{
    public static IEndpointRouteBuilder MapSubjectEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/subjects").RequireAuthorization();

        group.MapGet("/", async (int? grade, ClaimsPrincipal user, ISender sender, CancellationToken ct) =>
        {
            var studentGrade = grade ?? GetGradeFromClaims(user);
            var result = await sender.Send(new GetSubjectsQuery(studentGrade), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapGet("/{subjectId:guid}/topics", async (Guid subjectId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetTopicsQuery(subjectId), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapGet("/topics/{topicId:guid}/notes", async (Guid topicId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetNotesQuery(topicId), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapPost("/topics/{topicId:guid}/notes", async (Guid topicId, CreateNoteRequest request, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new CreateNoteCommand(topicId, request.Title, request.Content), ct);
            return result.Match(note => Results.Created($"/api/subjects/topics/{topicId}/notes", note), errors => errors.ToProblemResult());
        });

        group.MapGet("/topics/{topicId:guid}/questions", async (Guid topicId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetQuestionsQuery(topicId), ct);
            return result.Match(Results.Ok, errors => errors.ToProblemResult());
        });

        group.MapPost("/topics/{topicId:guid}/questions", async (Guid topicId, CreateQuestionRequest request, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new CreateQuestionCommand(topicId, request.QuestionText, request.AnswerText, request.Difficulty), ct);
            return result.Match(q => Results.Created($"/api/subjects/topics/{topicId}/questions", q), errors => errors.ToProblemResult());
        });

        return builder;
    }

    private static int GetGradeFromClaims(ClaimsPrincipal user)
    {
        var claim = user.FindFirstValue("grade");
        return int.TryParse(claim, out var grade) ? grade : 10;
    }
}
