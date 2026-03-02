using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Subjects.Common;

namespace StudyQuest.API.Features.Subjects.GetSubjects;

public record GetSubjectsQuery(int Grade) : IRequest<ErrorOr<List<SubjectResponse>>>;

internal sealed class GetSubjectsQueryHandler : IRequestHandler<GetSubjectsQuery, ErrorOr<List<SubjectResponse>>>
{
    private readonly AppDbContext _db;

    public GetSubjectsQueryHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<List<SubjectResponse>>> Handle(GetSubjectsQuery request, CancellationToken ct)
    {
        var subjects = await _db.Subjects
            .Where(s => s.Grade == request.Grade)
            .Include(s => s.Topics)
            .OrderBy(s => s.Name)
            .Select(s => new SubjectResponse(s.Id, s.Name, s.Grade, s.Description, s.Color, s.Topics.Count))
            .ToListAsync(ct);

        return subjects;
    }
}
