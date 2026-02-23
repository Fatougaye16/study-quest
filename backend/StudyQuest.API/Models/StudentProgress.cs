namespace StudyQuest.API.Models;

public class StudentProgress
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid SubjectId { get; set; }
    public int XP { get; set; }
    public int Level { get; set; } = 1;
    public int Streak { get; set; }
    public int TotalStudyMinutes { get; set; }
    public DateTime? LastStudyDate { get; set; }

    // Navigation properties
    public Student Student { get; set; } = null!;
    public Subject Subject { get; set; } = null!;

    public void AddXP(int amount)
    {
        XP += amount;
        Level = (XP / 500) + 1;
    }
}
