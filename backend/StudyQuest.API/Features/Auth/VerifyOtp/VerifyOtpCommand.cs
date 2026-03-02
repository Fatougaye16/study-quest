using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Auth.Common;
using StudyQuest.API.Models;

namespace StudyQuest.API.Features.Auth.VerifyOtp;

public record VerifyOtpCommand(string PhoneNumber, string OtpCode) : IRequest<ErrorOr<AuthResponse>>;

internal sealed class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, ErrorOr<AuthResponse>>
{
    private readonly AppDbContext _dbContext;
    private readonly IMemoryCache _cache;
    private readonly AuthTokenService _tokenService;

    public VerifyOtpCommandHandler(AppDbContext dbContext, IMemoryCache cache, AuthTokenService tokenService)
    {
        _dbContext = dbContext;
        _cache = cache;
        _tokenService = tokenService;
    }

    public async Task<ErrorOr<AuthResponse>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = OtpHelper.BuildCacheKey(request.PhoneNumber);
        if (!_cache.TryGetValue(cacheKey, out string? storedHash))
        {
            return AuthErrors.InvalidOtp;
        }

        if (!string.Equals(storedHash, OtpHelper.Hash(request.OtpCode), StringComparison.Ordinal))
        {
            return AuthErrors.InvalidOtp;
        }

        _cache.Remove(cacheKey);

        var student = await _dbContext.Students.FirstOrDefaultAsync(
            s => s.PhoneNumber == request.PhoneNumber,
            cancellationToken);

        if (student is null)
        {
            student = new Student
            {
                Id = Guid.NewGuid(),
                PhoneNumber = request.PhoneNumber,
                Grade = 10,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Students.AddAsync(student, cancellationToken);
        }

        student.LastLoginAt = DateTime.UtcNow;

        var tokens = _tokenService.GenerateTokens(student);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return tokens;
    }
}
