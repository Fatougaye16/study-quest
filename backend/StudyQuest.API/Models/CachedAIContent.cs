namespace StudyQuest.API.Models;

public enum AIContentType
{
    Summary = 0,
    Explanation = 1,
    Quiz = 2,
    Flashcards = 3
}

public class CachedAIContent
{
    public Guid Id { get; set; }
    public AIContentType ContentType { get; set; }
    public Guid TopicId { get; set; }
    public string InputHash { get; set; } = string.Empty;
    public string ResponseJson { get; set; } = string.Empty;
    public int StudentGrade { get; set; }
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Topic Topic { get; set; } = null!;
}
