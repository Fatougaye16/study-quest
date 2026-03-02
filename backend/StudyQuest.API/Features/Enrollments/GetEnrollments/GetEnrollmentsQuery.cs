using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Enrollments.Common;

namespace StudyQuest.API.Features.Enrollments.GetEnrollments;

public record GetEnrollmentsQuery(Guid StudentId) : IRequest<ErrorOr<List<EnrollmentResponse>>>;

internal sealed class GetEnrollmentsQueryHandler : IRequestHandler<GetEnrollmentsQuery, ErrorOr<List<EnrollmentResponse>>>
{
    private readonly AppDbContext _db;

    public GetEnrollmentsQueryHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<List<EnrollmentResponse>>> Handle(GetEnrollmentsQuery request, CancellationToken ct)
    {
        var enrollments = await _db.Enrollments
            .Where(e => e.StudentId == request.StudentId)
            .Include(e => e.Subject)
            .OrderBy(e => e.Subject.Name)
            .Select(e => new EnrollmentResponse(e.Id, e.SubjectId, e.Subject.Name, e.Subject.Color, e.Subject.Grade, e.EnrolledAt))
            .ToListAsync(ct);

        return enrollments;
    }
}
