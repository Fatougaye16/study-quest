namespace StudyQuest.API.Models;

public class Question
{
    public Guid Id { get; set; }
    public Guid TopicId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string AnswerText { get; set; } = string.Empty;
    public int Difficulty { get; set; } = 1; // 1 = Easy, 2 = Medium, 3 = Hard
    public bool IsAIGenerated { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Topic Topic { get; set; } = null!;
}
