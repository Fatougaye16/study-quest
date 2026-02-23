using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.DTOs.Enrollments;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Services.Implementations;

public class EnrollmentService : IEnrollmentService
{
    private readonly AppDbContext _db;
    private readonly IProgressService _progressService;

    public EnrollmentService(AppDbContext db, IProgressService progressService)
    {
        _db = db;
        _progressService = progressService;
    }

    public async Task<List<EnrollmentDto>> GetEnrollmentsAsync(Guid studentId)
    {
        return await _db.Enrollments
            .Where(e => e.StudentId == studentId)
            .Include(e => e.Subject)
            .OrderBy(e => e.Subject.Name)
            .Select(e => new EnrollmentDto(
                e.Id, e.SubjectId, e.Subject.Name, e.Subject.Color, e.Subject.Grade, e.EnrolledAt
            ))
            .ToListAsync();
    }

    public async Task<EnrollmentDto> EnrollAsync(Guid studentId, CreateEnrollmentDto dto)
    {
        var subject = await _db.Subjects.FindAsync(dto.SubjectId)
            ?? throw new InvalidOperationException("Subject not found");

        // Check if already enrolled
        var existing = await _db.Enrollments
            .AnyAsync(e => e.StudentId == studentId && e.SubjectId == dto.SubjectId);

        if (existing)
            throw new InvalidOperationException("Already enrolled in this subject");

        var enrollment = new Enrollment
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            SubjectId = dto.SubjectId
        };

        _db.Enrollments.Add(enrollment);
        await _db.SaveChangesAsync();

        // Award XP
        await _progressService.AddXPAsync(studentId, dto.SubjectId, 30);
        await _progressService.CheckAndUnlockAchievementsAsync(studentId);

        return new EnrollmentDto(
            enrollment.Id, enrollment.SubjectId, subject.Name, subject.Color, subject.Grade, enrollment.EnrolledAt
        );
    }

    public async Task<bool> UnenrollAsync(Guid studentId, Guid enrollmentId)
    {
        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(e => e.Id == enrollmentId && e.StudentId == studentId);

        if (enrollment == null) return false;

        _db.Enrollments.Remove(enrollment);
        await _db.SaveChangesAsync();
        return true;
    }
}
