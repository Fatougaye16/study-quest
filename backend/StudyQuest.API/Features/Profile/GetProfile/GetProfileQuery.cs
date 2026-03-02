using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Auth.Common;

namespace StudyQuest.API.Features.Profile.GetProfile;

public record GetProfileQuery(Guid StudentId) : IRequest<ErrorOr<StudentResponse>>;

internal sealed class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ErrorOr<StudentResponse>>
{
    private readonly AppDbContext _dbContext;

    public GetProfileQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<StudentResponse>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var student = await _dbContext.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(student => student.Id == request.StudentId, cancellationToken);

        if (student is null)
        {
            return AuthErrors.StudentNotFound;
        }

        return student.ToResponse();
    }
}
