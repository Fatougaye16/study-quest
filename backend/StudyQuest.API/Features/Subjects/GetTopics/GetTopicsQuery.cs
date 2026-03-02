using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Subjects.Common;

namespace StudyQuest.API.Features.Subjects.GetTopics;

public record GetTopicsQuery(Guid SubjectId) : IRequest<ErrorOr<List<TopicResponse>>>;

internal sealed class GetTopicsQueryHandler : IRequestHandler<GetTopicsQuery, ErrorOr<List<TopicResponse>>>
{
    private readonly AppDbContext _db;

    public GetTopicsQueryHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<List<TopicResponse>>> Handle(GetTopicsQuery request, CancellationToken ct)
    {
        var topics = await _db.Topics
            .Where(t => t.SubjectId == request.SubjectId)
            .Include(t => t.Notes)
            .Include(t => t.Questions)
            .OrderBy(t => t.Order)
            .Select(t => new TopicResponse(t.Id, t.SubjectId, t.Name, t.Order, t.Description, t.Notes.Count, t.Questions.Count))
            .ToListAsync(ct);

        return topics;
    }
}
