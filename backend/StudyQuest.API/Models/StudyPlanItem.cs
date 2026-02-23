namespace StudyQuest.API.Models;

public class StudyPlanItem
{
    public Guid Id { get; set; }
    public Guid StudyPlanId { get; set; }
    public Guid TopicId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Navigation properties
    public StudyPlan StudyPlan { get; set; } = null!;
    public Topic Topic { get; set; } = null!;
}
