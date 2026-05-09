using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.QuestionBank.Common;
using StudyQuest.API.Models;

namespace StudyQuest.API.Features.QuestionBank.DeletePastPaper;

public record DeletePastPaperCommand(Guid StudentId, Guid PastPaperId)
    : IRequest<ErrorOr<Deleted>>;

internal sealed class DeletePastPaperCommandHandler : IRequestHandler<DeletePastPaperCommand, ErrorOr<Deleted>>
{
    private readonly AppDbContext _db;

    public DeletePastPaperCommandHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<Deleted>> Handle(DeletePastPaperCommand request, CancellationToken ct)
    {
        var student = await _db.Students.FindAsync([request.StudentId], ct);
        if (student is null) return QuestionBankErrors.StudentNotFound;
        if (!student.IsAdmin) return QuestionBankErrors.NotAdmin;

        var paper = await _db.PastPapers.FindAsync([request.PastPaperId], ct);
        if (paper is null) return QuestionBankErrors.PaperNotFound;

        _db.PastPapers.Remove(paper);
        await _db.SaveChangesAsync(ct);

        // Invalidate cached PDF
        await _db.CachedDownloads
            .Where(c => c.ContentType == DownloadContentType.PastPaper && c.SourceId == request.PastPaperId)
            .ExecuteDeleteAsync(ct);

        return Result.Deleted;
    }
}
