using MediatR;
using Microsoft.EntityFrameworkCore;
using ErrorOr;
using StudyQuest.API.Data;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Features.Downloads.DownloadFlashcards;

public record DownloadFlashcardsQuery(Guid TopicId, Guid StudentId) : IRequest<ErrorOr<DownloadResult>>;

public record DownloadResult(byte[] PdfData, string FileName);

public class DownloadFlashcardsHandler(AppDbContext db, IPdfGeneratorService pdf) : IRequestHandler<DownloadFlashcardsQuery, ErrorOr<DownloadResult>>
{
    public async Task<ErrorOr<DownloadResult>> Handle(DownloadFlashcardsQuery request, CancellationToken ct)
    {
        // No global cache — flashcard content is student-specific
        var topic = await db.Topics
            .Include(t => t.Subject)
            .FirstOrDefaultAsync(t => t.Id == request.TopicId, ct);

        if (topic is null)
            return Error.NotFound("Topic.NotFound", "Topic not found.");

        var flashcards = await db.Flashcards
            .Where(f => f.TopicId == request.TopicId && (f.StudentId == null || f.StudentId == request.StudentId))
            .OrderBy(f => f.CreatedAt)
            .ToListAsync(ct);

        if (flashcards.Count == 0)
            return Error.NotFound("Flashcards.Empty", "No flashcards available for this topic.");

        var pdfBytes = pdf.GenerateFlashcardsPdf(topic.Name, flashcards);
        var fileName = $"{topic.Subject.Name}_{topic.Name}_Flashcards.pdf";

        return new DownloadResult(pdfBytes, fileName);
    }
}
