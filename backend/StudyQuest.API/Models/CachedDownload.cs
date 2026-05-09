namespace StudyQuest.API.Models;

public enum DownloadContentType
{
    PastPaper = 0,
    Notes = 1,
    Flashcards = 2,
    StudyPlan = 3
}

public class CachedDownload
{
    public Guid Id { get; set; }
    public DownloadContentType ContentType { get; set; }
    public Guid SourceId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public byte[] PdfData { get; set; } = [];
    public string ContentHash { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public long FileSizeBytes { get; set; }
}
