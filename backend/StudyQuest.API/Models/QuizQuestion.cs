using System.Text.Json;

namespace StudyQuest.API.Models;

public class QuizQuestion
{
    public Guid Id { get; set; }
    public Guid QuizId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string OptionsJson { get; set; } = "[]"; // JSON array of strings
    public string CorrectAnswer { get; set; } = string.Empty;
    public string? StudentAnswer { get; set; }
    public string? Explanation { get; set; }

    // Navigation properties
    public Quiz Quiz { get; set; } = null!;

    // Helper methods
    public List<string> GetOptions() =>
        JsonSerializer.Deserialize<List<string>>(OptionsJson) ?? [];

    public void SetOptions(List<string> options) =>
        OptionsJson = JsonSerializer.Serialize(options);

    public bool IsCorrect => StudentAnswer == CorrectAnswer;
}
