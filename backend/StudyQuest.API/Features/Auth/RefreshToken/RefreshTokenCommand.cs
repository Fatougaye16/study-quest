using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Auth.Common;

namespace StudyQuest.API.Features.Auth.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<ErrorOr<AuthResponse>>;

internal sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<AuthResponse>>
{
    private readonly AppDbContext _dbContext;
    private readonly AuthTokenService _tokenService;

    public RefreshTokenCommandHandler(AppDbContext dbContext, AuthTokenService tokenService)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
    }

    public async Task<ErrorOr<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var student = await _dbContext.Students
            .FirstOrDefaultAsync(student => student.RefreshToken == request.RefreshToken, cancellationToken);

        if (student is null)
        {
            return AuthErrors.InvalidRefreshToken;
        }

        if (student.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return AuthErrors.RefreshTokenExpired;
        }

        var tokens = _tokenService.GenerateTokens(student);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return tokens;
    }
}
