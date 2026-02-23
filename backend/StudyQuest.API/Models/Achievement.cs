namespace StudyQuest.API.Models;

public class Achievement
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string Type { get; set; } = string.Empty; // e.g. "first_session", "streak_7"
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = "🏆";
    public int XPReward { get; set; }
    public DateTime UnlockedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Student Student { get; set; } = null!;
}
