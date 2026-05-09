using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ErrorOr;
using StudyQuest.API.Data;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Features.Downloads.DownloadNotes;

public record DownloadNotesQuery(Guid TopicId) : IRequest<ErrorOr<DownloadResult>>;

public record DownloadResult(byte[] PdfData, string FileName);

public class DownloadNotesHandler(AppDbContext db, IPdfGeneratorService pdf) : IRequestHandler<DownloadNotesQuery, ErrorOr<DownloadResult>>
{
    public async Task<ErrorOr<DownloadResult>> Handle(DownloadNotesQuery request, CancellationToken ct)
    {
        var cached = await db.CachedDownloads
            .FirstOrDefaultAsync(c => c.ContentType == DownloadContentType.Notes && c.SourceId == request.TopicId, ct);

        if (cached is not null)
            return new DownloadResult(cached.PdfData, cached.FileName);

        var topic = await db.Topics
            .Include(t => t.Subject)
            .FirstOrDefaultAsync(t => t.Id == request.TopicId, ct);

        if (topic is null)
            return Error.NotFound("Topic.NotFound", "Topic not found.");

        var notes = await db.Notes
            .Where(n => n.TopicId == request.TopicId)
            .OrderByDescending(n => n.IsOfficial)
            .ThenBy(n => n.CreatedAt)
            .ToListAsync(ct);

        if (notes.Count == 0)
            return Error.NotFound("Notes.Empty", "No notes available for this topic.");

        var pdfBytes = pdf.GenerateNotesPdf(topic.Subject.Name, topic.Name, notes);
        var fileName = $"{topic.Subject.Name}_{topic.Name}_Notes.pdf";
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(string.Join("|", notes.Select(n => n.Id)))));

        db.CachedDownloads.Add(new CachedDownload
        {
            ContentType = DownloadContentType.Notes,
            SourceId = request.TopicId,
            FileName = fileName,
            PdfData = pdfBytes,
            ContentHash = hash,
            FileSizeBytes = pdfBytes.Length
        });
        await db.SaveChangesAsync(ct);

        return new DownloadResult(pdfBytes, fileName);
    }
}
