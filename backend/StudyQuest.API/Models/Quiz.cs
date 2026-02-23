namespace StudyQuest.API.Models;

public class Quiz
{
    public Guid Id { get; set; }
    public Guid TopicId { get; set; }
    public Guid StudentId { get; set; }
    public int Score { get; set; }
    public int TotalQuestions { get; set; }
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Topic Topic { get; set; } = null!;
    public Student Student { get; set; } = null!;
    public ICollection<QuizQuestion> Questions { get; set; } = [];
}
