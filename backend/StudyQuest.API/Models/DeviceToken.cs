namespace StudyQuest.API.Models;

public class DeviceToken
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string Token { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty; // "ios", "android"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Student Student { get; set; } = null!;
}
