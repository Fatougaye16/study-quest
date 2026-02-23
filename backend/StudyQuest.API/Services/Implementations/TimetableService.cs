using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.DTOs.Timetable;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Services.Implementations;

public class TimetableService : ITimetableService
{
    private readonly AppDbContext _db;
    private readonly IProgressService _progressService;

    public TimetableService(AppDbContext db, IProgressService progressService)
    {
        _db = db;
        _progressService = progressService;
    }

    public async Task<List<TimetableEntryDto>> GetTimetableAsync(Guid studentId)
    {
        return await _db.TimetableEntries
            .Where(t => t.StudentId == studentId)
            .Include(t => t.Subject)
            .OrderBy(t => t.DayOfWeek)
            .ThenBy(t => t.StartTime)
            .Select(t => new TimetableEntryDto(
                t.Id, t.SubjectId, t.Subject.Name, t.Subject.Color,
                t.DayOfWeek, t.StartTime, t.EndTime, t.Location
            ))
            .ToListAsync();
    }

    public async Task<TimetableEntryDto> CreateEntryAsync(Guid studentId, CreateTimetableEntryDto dto)
    {
        var subject = await _db.Subjects.FindAsync(dto.SubjectId)
            ?? throw new InvalidOperationException("Subject not found");

        var entry = new TimetableEntry
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            SubjectId = dto.SubjectId,
            DayOfWeek = dto.DayOfWeek,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Location = dto.Location
        };

        _db.TimetableEntries.Add(entry);
        await _db.SaveChangesAsync();

        // Award XP
        await _progressService.AddXPAsync(studentId, dto.SubjectId, 20);

        return new TimetableEntryDto(
            entry.Id, entry.SubjectId, subject.Name, subject.Color,
            entry.DayOfWeek, entry.StartTime, entry.EndTime, entry.Location
        );
    }

    public async Task<TimetableEntryDto?> UpdateEntryAsync(Guid studentId, Guid entryId, UpdateTimetableEntryDto dto)
    {
        var entry = await _db.TimetableEntries
            .Include(t => t.Subject)
            .FirstOrDefaultAsync(t => t.Id == entryId && t.StudentId == studentId);

        if (entry == null) return null;

        var subject = await _db.Subjects.FindAsync(dto.SubjectId)
            ?? throw new InvalidOperationException("Subject not found");

        entry.SubjectId = dto.SubjectId;
        entry.DayOfWeek = dto.DayOfWeek;
        entry.StartTime = dto.StartTime;
        entry.EndTime = dto.EndTime;
        entry.Location = dto.Location;

        await _db.SaveChangesAsync();

        return new TimetableEntryDto(
            entry.Id, entry.SubjectId, subject.Name, subject.Color,
            entry.DayOfWeek, entry.StartTime, entry.EndTime, entry.Location
        );
    }

    public async Task<bool> DeleteEntryAsync(Guid studentId, Guid entryId)
    {
        var entry = await _db.TimetableEntries
            .FirstOrDefaultAsync(t => t.Id == entryId && t.StudentId == studentId);

        if (entry == null) return false;

        _db.TimetableEntries.Remove(entry);
        await _db.SaveChangesAsync();
        return true;
    }
}
