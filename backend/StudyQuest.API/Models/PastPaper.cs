namespace StudyQuest.API.Models;

public enum ExamType
{
    WASSCE = 0,
    BECE = 1,
    NECO = 2
}

public class PastPaper
{
    public Guid Id { get; set; }
    public Guid SubjectId { get; set; }
    public int Year { get; set; }
    public ExamType ExamType { get; set; }
    public int PaperNumber { get; set; } = 1;
    public string Title { get; set; } = string.Empty;
    public Guid CreatedByStudentId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Subject Subject { get; set; } = null!;
    public Student CreatedBy { get; set; } = null!;
    public ICollection<PastQuestion> Questions { get; set; } = [];
}
