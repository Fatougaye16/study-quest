namespace StudyQuest.API.Models;

public enum NoteSourceType
{
    Manual = 0,
    Pdf = 1,
    Document = 2,
    Image = 3
}

public class Note
{
    public Guid Id { get; set; }
    public Guid TopicId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty; // Markdown content
    public bool IsAIGenerated { get; set; }
    public NoteSourceType SourceType { get; set; } = NoteSourceType.Manual;
    public string? OriginalFileName { get; set; }
    public bool IsOfficial { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public Topic Topic { get; set; } = null!;
}
