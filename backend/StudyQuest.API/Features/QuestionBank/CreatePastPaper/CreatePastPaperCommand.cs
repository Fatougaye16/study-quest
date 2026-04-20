using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.QuestionBank.Common;
using StudyQuest.API.Models;

namespace StudyQuest.API.Features.QuestionBank.CreatePastPaper;

public record CreatePastPaperCommand(Guid StudentId, Guid SubjectId, int Year, string ExamType, int PaperNumber, string Title)
    : IRequest<ErrorOr<PastPaperResponse>>;

internal sealed class CreatePastPaperCommandHandler : IRequestHandler<CreatePastPaperCommand, ErrorOr<PastPaperResponse>>
{
    private readonly AppDbContext _db;

    public CreatePastPaperCommandHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<PastPaperResponse>> Handle(CreatePastPaperCommand request, CancellationToken ct)
    {
        var student = await _db.Students.FindAsync([request.StudentId], ct);
        if (student is null) return QuestionBankErrors.StudentNotFound;
        if (!student.IsAdmin) return QuestionBankErrors.NotAdmin;

        var subject = await _db.Subjects.FindAsync([request.SubjectId], ct);
        if (subject is null) return QuestionBankErrors.SubjectNotFound;

        if (!Enum.TryParse<ExamType>(request.ExamType, true, out var examType))
            return Error.Validation("QuestionBank.InvalidExamType", $"Invalid exam type: {request.ExamType}. Valid: WASSCE, BECE, NECO.");

        var exists = await _db.PastPapers.AnyAsync(p =>
            p.SubjectId == request.SubjectId && p.Year == request.Year &&
            p.ExamType == examType && p.PaperNumber == request.PaperNumber, ct);
        if (exists) return QuestionBankErrors.PaperAlreadyExists;

        var paper = new PastPaper
        {
            Id = Guid.NewGuid(),
            SubjectId = request.SubjectId,
            Year = request.Year,
            ExamType = examType,
            PaperNumber = request.PaperNumber,
            Title = request.Title,
            CreatedByStudentId = request.StudentId
        };

        _db.PastPapers.Add(paper);
        await _db.SaveChangesAsync(ct);

        return new PastPaperResponse(
            paper.Id, paper.SubjectId, subject.Name, paper.Year,
            paper.ExamType.ToString(), paper.PaperNumber, paper.Title, 0, paper.CreatedAt);
    }
}
