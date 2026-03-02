using ErrorOr;
using MediatR;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Auth.Common;

namespace StudyQuest.API.Features.Auth.Logout;

public record LogoutCommand(Guid StudentId) : IRequest<ErrorOr<Unit>>;

internal sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, ErrorOr<Unit>>
{
    private readonly AppDbContext _dbContext;

    public LogoutCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<Unit>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var student = await _dbContext.Students.FindAsync([request.StudentId], cancellationToken);
        if (student is null)
        {
            return AuthErrors.StudentNotFound;
        }

        student.RefreshToken = null;
        student.RefreshTokenExpiryTime = null;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
