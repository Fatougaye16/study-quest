using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ErrorOr;
using StudyQuest.API.Data;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Features.Downloads.DownloadPastPaper;

public record DownloadPastPaperQuery(Guid PaperId) : IRequest<ErrorOr<DownloadResult>>;

public record DownloadResult(byte[] PdfData, string FileName);

public class DownloadPastPaperHandler(AppDbContext db, IPdfGeneratorService pdf) : IRequestHandler<DownloadPastPaperQuery, ErrorOr<DownloadResult>>
{
    public async Task<ErrorOr<DownloadResult>> Handle(DownloadPastPaperQuery request, CancellationToken ct)
    {
        // Check cache first
        var cached = await db.CachedDownloads
            .FirstOrDefaultAsync(c => c.ContentType == DownloadContentType.PastPaper && c.SourceId == request.PaperId, ct);

        if (cached is not null)
            return new DownloadResult(cached.PdfData, cached.FileName);

        var paper = await db.PastPapers
            .Include(p => p.Subject)
            .Include(p => p.Questions)
            .FirstOrDefaultAsync(p => p.Id == request.PaperId, ct);

        if (paper is null)
            return Error.NotFound("PastPaper.NotFound", "Past paper not found.");

        var questions = paper.Questions.OrderBy(q => q.QuestionNumber).ToList();
        var pdfBytes = pdf.GeneratePastPaperPdf(paper, paper.Subject.Name, questions);
        var fileName = $"{paper.Subject.Name}_{paper.ExamType}_{paper.Year}_P{paper.PaperNumber}.pdf";
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(string.Join("|", questions.Select(q => q.Id)))));

        db.CachedDownloads.Add(new CachedDownload
        {
            ContentType = DownloadContentType.PastPaper,
            SourceId = request.PaperId,
            FileName = fileName,
            PdfData = pdfBytes,
            ContentHash = hash,
            FileSizeBytes = pdfBytes.Length
        });
        await db.SaveChangesAsync(ct);

        return new DownloadResult(pdfBytes, fileName);
    }
}
