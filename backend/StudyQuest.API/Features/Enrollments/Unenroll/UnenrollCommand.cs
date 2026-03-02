using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Enrollments.Common;

namespace StudyQuest.API.Features.Enrollments.Unenroll;

public record UnenrollCommand(Guid StudentId, Guid EnrollmentId) : IRequest<ErrorOr<Unit>>;

internal sealed class UnenrollCommandHandler : IRequestHandler<UnenrollCommand, ErrorOr<Unit>>
{
    private readonly AppDbContext _db;

    public UnenrollCommandHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<Unit>> Handle(UnenrollCommand request, CancellationToken ct)
    {
        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(e => e.Id == request.EnrollmentId && e.StudentId == request.StudentId, ct);

        if (enrollment is null)
            return EnrollmentErrors.EnrollmentNotFound;

        _db.Enrollments.Remove(enrollment);
        await _db.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
