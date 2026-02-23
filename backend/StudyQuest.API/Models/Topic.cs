namespace StudyQuest.API.Models;

public class Topic
{
    public Guid Id { get; set; }
    public Guid SubjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
    public string Description { get; set; } = string.Empty;

    // Navigation properties
    public Subject Subject { get; set; } = null!;
    public ICollection<Note> Notes { get; set; } = [];
    public ICollection<Question> Questions { get; set; } = [];
    public ICollection<Flashcard> Flashcards { get; set; } = [];
}
