using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Auth.Common;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Features.Auth.Login;

// Returns either AuthResponse (tokens) or LoginOtpRequiredResponse (OTP needed)
public record LoginCommand(string PhoneNumber, string Password) : IRequest<ErrorOr<object>>;

internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<object>>
{
    private readonly AppDbContext _db;
    private readonly AuthTokenService _tokenService;
    private readonly IOtpService _otpService;

    public LoginCommandHandler(AppDbContext db, AuthTokenService tokenService, IOtpService otpService)
    {
        _db = db;
        _tokenService = tokenService;
        _otpService = otpService;
    }

    public async Task<ErrorOr<object>> Handle(LoginCommand request, CancellationToken ct)
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

        // If OTP is enabled, send OTP and require verification before issuing tokens
        if (student.IsOtpEnabled)
        {
            await _db.SaveChangesAsync(ct);

            var otpResult = await _otpService.GenerateAndSendOtpAsync(request.PhoneNumber, ct);
            if (otpResult.IsError)
                return otpResult.Errors;

            return new LoginOtpRequiredResponse(
                OtpRequired: true,
                Message: "OTP has been sent to your phone. Please verify to complete login.",
                PhoneNumber: request.PhoneNumber);
        }

        var tokens = _tokenService.GenerateTokens(student);
        await _db.SaveChangesAsync(ct);

        return tokens;
    }
}
