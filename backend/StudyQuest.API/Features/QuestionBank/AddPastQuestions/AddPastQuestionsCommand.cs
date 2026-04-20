using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.QuestionBank.Common;
using StudyQuest.API.Models;

namespace StudyQuest.API.Features.QuestionBank.AddPastQuestions;

public record AddPastQuestionsCommand(Guid StudentId, Guid PastPaperId, List<PastQuestionInput> Questions)
    : IRequest<ErrorOr<List<PastQuestionResponse>>>;

internal sealed class AddPastQuestionsCommandHandler
    : IRequestHandler<AddPastQuestionsCommand, ErrorOr<List<PastQuestionResponse>>>
{
    private readonly AppDbContext _db;

    public AddPastQuestionsCommandHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<List<PastQuestionResponse>>> Handle(AddPastQuestionsCommand request, CancellationToken ct)
    {
        var student = await _db.Students.FindAsync([request.StudentId], ct);
        if (student is null) return QuestionBankErrors.StudentNotFound;
        if (!student.IsAdmin) return QuestionBankErrors.NotAdmin;

        var paper = await _db.PastPapers.FindAsync([request.PastPaperId], ct);
        if (paper is null) return QuestionBankErrors.PaperNotFound;

        var questions = request.Questions.Select(q => new PastQuestion
        {
            Id = Guid.NewGuid(),
            PastPaperId = request.PastPaperId,
            TopicId = q.TopicId,
            QuestionNumber = q.QuestionNumber,
            QuestionText = q.QuestionText,
            AnswerText = q.AnswerText,
            Marks = q.Marks,
            Difficulty = q.Difficulty
        }).ToList();

        _db.PastQuestions.AddRange(questions);
        await _db.SaveChangesAsync(ct);

        // Invalidate cached PDF for this paper
        await _db.CachedDownloads
            .Where(c => c.ContentType == DownloadContentType.PastPaper && c.SourceId == request.PastPaperId)
            .ExecuteDeleteAsync(ct);

        // Load topic names for response
        var topicIds = questions.Where(q => q.TopicId.HasValue).Select(q => q.TopicId!.Value).Distinct().ToList();
        var topicNames = await _db.Topics.Where(t => topicIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, t => t.Name, ct);

        return questions.Select(q => new PastQuestionResponse(
            q.Id, q.QuestionNumber, q.QuestionText, q.AnswerText,
            q.Marks, q.ImageUrl, q.Difficulty,
            q.TopicId.HasValue && topicNames.TryGetValue(q.TopicId.Value, out var name) ? name : null
        )).ToList();
    }
}
