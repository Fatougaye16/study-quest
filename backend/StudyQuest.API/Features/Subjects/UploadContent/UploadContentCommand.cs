using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Subjects.Common;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Features.Subjects.UploadContent;

public record UploadContentCommand(Guid StudentId, Guid TopicId, IFormFile File)
    : IRequest<ErrorOr<NoteResponse>>;

internal sealed class UploadContentCommandHandler : IRequestHandler<UploadContentCommand, ErrorOr<NoteResponse>>
{
    private readonly AppDbContext _db;
    private readonly ITextExtractorService _extractor;
    private readonly ILogger<UploadContentCommandHandler> _logger;

    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf", ".docx", ".txt", ".png", ".jpg", ".jpeg"
    };

    private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB

    public UploadContentCommandHandler(AppDbContext db, ITextExtractorService extractor, ILogger<UploadContentCommandHandler> logger)
    {
        _db = db;
        _extractor = extractor;
        _logger = logger;
    }

    public async Task<ErrorOr<NoteResponse>> Handle(UploadContentCommand request, CancellationToken ct)
    {
        var student = await _db.Students.FindAsync([request.StudentId], ct);
        if (student is null)
            return Error.NotFound("Upload.StudentNotFound", "Student not found.");

        var topic = await _db.Topics.FindAsync([request.TopicId], ct);
        if (topic is null)
            return SubjectErrors.TopicNotFound;

        var file = request.File;

        if (file.Length == 0)
            return Error.Validation("Upload.EmptyFile", "The uploaded file is empty.");

        if (file.Length > MaxFileSize)
            return Error.Validation("Upload.FileTooLarge", "File size exceeds the 10 MB limit.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            return Error.Validation("Upload.UnsupportedType",
                $"Unsupported file type: {extension}. Allowed: {string.Join(", ", AllowedExtensions)}");

        string extractedText;
        try
        {
            using var stream = file.OpenReadStream();
            extractedText = await _extractor.ExtractAsync(stream, file.FileName, file.ContentType);
        }
        catch (InvalidOperationException ex)
        {
            return Error.Failure("Upload.ExtractionFailed", ex.Message);
        }

        if (string.IsNullOrWhiteSpace(extractedText))
            return Error.Failure("Upload.NoContent", "Could not extract any text from the uploaded file.");

        var sourceType = extension switch
        {
            ".pdf" => NoteSourceType.Pdf,
            ".docx" => NoteSourceType.Document,
            ".txt" => NoteSourceType.Document,
            ".png" or ".jpg" or ".jpeg" => NoteSourceType.Image,
            _ => NoteSourceType.Manual
        };

        var title = Path.GetFileNameWithoutExtension(file.FileName);
        if (title.Length > 280) title = title[..280];

        var note = new Note
        {
            Id = Guid.NewGuid(),
            TopicId = request.TopicId,
            Title = title,
            Content = extractedText,
            IsAIGenerated = false,
            SourceType = sourceType,
            OriginalFileName = file.FileName,
            IsOfficial = student.IsAdmin
        };

        _db.Notes.Add(note);
        await _db.SaveChangesAsync(ct);

        // Invalidate AI cache for this topic (content changed, regenerate next time)
        await _db.CachedAIContents.Where(c => c.TopicId == request.TopicId).ExecuteDeleteAsync(ct);
        await _db.CachedDownloads.Where(c => c.ContentType == DownloadContentType.Notes && c.SourceId == request.TopicId).ExecuteDeleteAsync(ct);

        _logger.LogInformation("Content uploaded for topic {TopicId}: {FileName} ({SourceType}, {Length} chars)",
            request.TopicId, file.FileName, sourceType, extractedText.Length);

        return new NoteResponse(note.Id, note.TopicId, note.Title, note.Content, note.IsAIGenerated,
            (int)note.SourceType, note.OriginalFileName, note.IsOfficial, note.CreatedAt);
    }
}
