using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Enrollments.Common;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Features.Enrollments.Enroll;

public record EnrollCommand(Guid StudentId, Guid SubjectId) : IRequest<ErrorOr<EnrollmentResponse>>;

internal sealed class EnrollCommandHandler : IRequestHandler<EnrollCommand, ErrorOr<EnrollmentResponse>>
{
    private readonly AppDbContext _db;
    private readonly IProgressService _progressService;

    public EnrollCommandHandler(AppDbContext db, IProgressService progressService)
    {
        _db = db;
        _progressService = progressService;
    }

    public async Task<ErrorOr<EnrollmentResponse>> Handle(EnrollCommand request, CancellationToken ct)
    {
        var subject = await _db.Subjects.FindAsync([request.SubjectId], ct);
        if (subject is null)
            return EnrollmentErrors.SubjectNotFound;

        var exists = await _db.Enrollments
            .AnyAsync(e => e.StudentId == request.StudentId && e.SubjectId == request.SubjectId, ct);

        if (exists)
            return EnrollmentErrors.AlreadyEnrolled;

        var enrollment = new Enrollment
        {
            Id = Guid.NewGuid(),
            StudentId = request.StudentId,
            SubjectId = request.SubjectId
        };

        _db.Enrollments.Add(enrollment);
        await _db.SaveChangesAsync(ct);

        await _progressService.AddXPAsync(request.StudentId, request.SubjectId, 30);
        await _progressService.CheckAndUnlockAchievementsAsync(request.StudentId);

        return new EnrollmentResponse(enrollment.Id, enrollment.SubjectId, subject.Name, subject.Color, subject.Grade, enrollment.EnrolledAt);
    }
}
