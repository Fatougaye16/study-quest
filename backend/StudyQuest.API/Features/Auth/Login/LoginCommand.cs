using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Auth.Common;
using StudyQuest.API.Models;

namespace StudyQuest.API.Features.Auth.Login;

public record LoginCommand(string PhoneNumber, string Password) : IRequest<ErrorOr<AuthResponse>>;

internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<AuthResponse>>
{
    private readonly AppDbContext _db;
    private readonly AuthTokenService _tokenService;

    public LoginCommandHandler(AppDbContext db, AuthTokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }

    public async Task<ErrorOr<AuthResponse>> Handle(LoginCommand request, CancellationToken ct)
    {
        var student = await _db.Students.FirstOrDefaultAsync(s => s.PhoneNumber == request.PhoneNumber, ct);
        if (student is null)
            return AuthErrors.InvalidCredentials;

        var hasher = new PasswordHasher<Student>();
        var verification = hasher.VerifyHashedPassword(student, student.PasswordHash, request.Password);

        if (verification == PasswordVerificationResult.Failed)
            return AuthErrors.InvalidCredentials;

        student.LastLoginAt = DateTime.UtcNow;

        // Rehash if the hasher indicates the hash needs upgrading
        if (verification == PasswordVerificationResult.SuccessRehashNeeded)
            student.PasswordHash = hasher.HashPassword(student, request.Password);

        var tokens = _tokenService.GenerateTokens(student);
        await _db.SaveChangesAsync(ct);

        return tokens;
    }
}
