using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.DTOs.StudyPlans;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Services.Implementations;

public class StudyPlanService : IStudyPlanService
{
    private readonly AppDbContext _db;
    private readonly IProgressService _progressService;

    public StudyPlanService(AppDbContext db, IProgressService progressService)
    {
        _db = db;
        _progressService = progressService;
    }

    public async Task<List<StudyPlanDto>> GetStudyPlansAsync(Guid studentId)
    {
        return await _db.StudyPlans
            .Where(p => p.StudentId == studentId)
            .Include(p => p.Subject)
            .Include(p => p.Items).ThenInclude(i => i.Topic)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => MapToDto(p))
            .ToListAsync();
    }

    public async Task<StudyPlanDto?> GetStudyPlanByIdAsync(Guid studentId, Guid planId)
    {
        var plan = await _db.StudyPlans
            .Where(p => p.Id == planId && p.StudentId == studentId)
            .Include(p => p.Subject)
            .Include(p => p.Items).ThenInclude(i => i.Topic)
            .FirstOrDefaultAsync();

        return plan == null ? null : MapToDto(plan);
    }

    public async Task<StudyPlanDto> CreateStudyPlanAsync(Guid studentId, CreateStudyPlanDto dto)
    {
        var subject = await _db.Subjects.FindAsync(dto.SubjectId)
            ?? throw new InvalidOperationException("Subject not found");

        var plan = new StudyPlan
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            SubjectId = dto.SubjectId,
            Title = dto.Title,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            IsAIGenerated = false
        };

        _db.StudyPlans.Add(plan);

        foreach (var itemDto in dto.Items)
        {
            var topic = await _db.Topics.FindAsync(itemDto.TopicId)
                ?? throw new InvalidOperationException($"Topic {itemDto.TopicId} not found");

            _db.StudyPlanItems.Add(new StudyPlanItem
            {
                Id = Guid.NewGuid(),
                StudyPlanId = plan.Id,
                TopicId = itemDto.TopicId,
                ScheduledDate = itemDto.ScheduledDate,
                DurationMinutes = itemDto.DurationMinutes,
                IsCompleted = false
            });
        }

        await _db.SaveChangesAsync();

        // Award XP for creating a plan
        await _progressService.AddXPAsync(studentId, dto.SubjectId, 30);

        // Reload with navigation properties
        return (await GetStudyPlanByIdAsync(studentId, plan.Id))!;
    }

    public async Task<bool> ToggleItemCompletionAsync(Guid studentId, Guid planId, Guid itemId)
    {
        var plan = await _db.StudyPlans
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == planId && p.StudentId == studentId);

        if (plan == null) return false;

        var item = plan.Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null) return false;

        item.IsCompleted = !item.IsCompleted;
        item.CompletedAt = item.IsCompleted ? DateTime.UtcNow : null;

        await _db.SaveChangesAsync();

        // Award XP for completing an item
        if (item.IsCompleted)
        {
            var xp = Math.Max(10, item.DurationMinutes / 10 * 10);
            await _progressService.AddXPAsync(studentId, plan.SubjectId, xp);
            await _progressService.CheckAndUnlockAchievementsAsync(studentId);
        }

        return true;
    }

    public async Task<bool> DeleteStudyPlanAsync(Guid studentId, Guid planId)
    {
        var plan = await _db.StudyPlans
            .FirstOrDefaultAsync(p => p.Id == planId && p.StudentId == studentId);

        if (plan == null) return false;

        _db.StudyPlans.Remove(plan);
        await _db.SaveChangesAsync();
        return true;
    }

    private static StudyPlanDto MapToDto(StudyPlan p)
    {
        var totalItems = p.Items.Count;
        var completedItems = p.Items.Count(i => i.IsCompleted);

        return new StudyPlanDto(
            Id: p.Id,
            SubjectId: p.SubjectId,
            SubjectName: p.Subject.Name,
            Title: p.Title,
            StartDate: p.StartDate,
            EndDate: p.EndDate,
            IsAIGenerated: p.IsAIGenerated,
            CreatedAt: p.CreatedAt,
            Items: p.Items.OrderBy(i => i.ScheduledDate).Select(i => new StudyPlanItemDto(
                Id: i.Id,
                TopicId: i.TopicId,
                TopicName: i.Topic.Name,
                ScheduledDate: i.ScheduledDate,
                DurationMinutes: i.DurationMinutes,
                IsCompleted: i.IsCompleted,
                CompletedAt: i.CompletedAt
            )).ToList(),
            CompletionPercentage: totalItems > 0 ? Math.Round((double)completedItems / totalItems * 100, 1) : 0
        );
    }
}
