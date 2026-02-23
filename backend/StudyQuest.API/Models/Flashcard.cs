namespace StudyQuest.API.Models;

public class Flashcard
{
    public Guid Id { get; set; }
    public Guid TopicId { get; set; }
    public Guid? StudentId { get; set; } // null = shared/template, set = personal
    public string Front { get; set; } = string.Empty;
    public string Back { get; set; } = string.Empty;
    public bool IsAIGenerated { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Topic Topic { get; set; } = null!;
    public Student? Student { get; set; }
}
