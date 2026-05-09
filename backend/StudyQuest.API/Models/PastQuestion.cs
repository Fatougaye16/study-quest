namespace StudyQuest.API.Models;

public class PastQuestion
{
    public Guid Id { get; set; }
    public Guid PastPaperId { get; set; }
    public Guid? TopicId { get; set; }
    public int QuestionNumber { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string? AnswerText { get; set; }
    public int? Marks { get; set; }
    public string? ImageUrl { get; set; }
    public int Difficulty { get; set; } = 1; // 1 = Easy, 2 = Medium, 3 = Hard

    // Navigation properties
    public PastPaper PastPaper { get; set; } = null!;
    public Topic? Topic { get; set; }
}
