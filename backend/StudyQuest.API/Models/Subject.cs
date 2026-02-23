namespace StudyQuest.API.Models;

public class Subject
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Grade { get; set; } // 10, 11, or 12
    public string Description { get; set; } = string.Empty;
    public string Color { get; set; } = "#8b5cf6";

    // Navigation properties
    public ICollection<Topic> Topics { get; set; } = [];
    public ICollection<Enrollment> Enrollments { get; set; } = [];
}
