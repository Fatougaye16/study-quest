namespace StudyQuest.API.Models;

public class Student
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Grade { get; set; } // 10, 11, or 12
    public int DailyGoalMinutes { get; set; } = 30;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }

    // OTP two-factor authentication
    public bool IsOtpEnabled { get; set; }

    // Refresh token for JWT
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    // Navigation properties
    public ICollection<Enrollment> Enrollments { get; set; } = [];
    public ICollection<TimetableEntry> TimetableEntries { get; set; } = [];
    public ICollection<StudyPlan> StudyPlans { get; set; } = [];
    public ICollection<StudySession> StudySessions { get; set; } = [];
    public ICollection<StudentProgress> ProgressRecords { get; set; } = [];
    public ICollection<Achievement> Achievements { get; set; } = [];
    public ICollection<Flashcard> Flashcards { get; set; } = [];
    public ICollection<Quiz> Quizzes { get; set; } = [];
    public ICollection<DeviceToken> DeviceTokens { get; set; } = [];
    public ICollection<Reminder> Reminders { get; set; } = [];
}
