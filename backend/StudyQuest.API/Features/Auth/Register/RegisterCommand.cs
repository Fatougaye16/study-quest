using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Auth.Common;
using StudyQuest.API.Models;

namespace StudyQuest.API.Features.Auth.Register;

public record RegisterCommand(string PhoneNumber, string Password, string FirstName, string LastName, int Grade, bool EnableOtp)
    : IRequest<ErrorOr<AuthResponse>>;

internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthResponse>>
{
    private readonly AppDbContext _db;
    private readonly AuthTokenService _tokenService;

    public RegisterCommandHandler(AppDbContext db, AuthTokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }

    public async Task<ErrorOr<AuthResponse>> Handle(RegisterCommand request, CancellationToken ct)
    {
        var exists = await _db.Students.AnyAsync(s => s.PhoneNumber == request.PhoneNumber, ct);
        if (exists)
            return AuthErrors.PhoneAlreadyRegistered;

        var hasher = new PasswordHasher<Student>();
        var student = new Student
        {
            Id = Guid.NewGuid(),
            PhoneNumber = request.PhoneNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Grade = request.Grade,
            IsOtpEnabled = request.EnableOtp,
            CreatedAt = DateTime.UtcNow,
            LastLoginAt = DateTime.UtcNow
        };

        student.PasswordHash = hasher.HashPassword(student, request.Password);

        _db.Students.Add(student);
        await _db.SaveChangesAsync(ct);

        var tokens = _tokenService.GenerateTokens(student);
        await _db.SaveChangesAsync(ct);

        return tokens;
    }
}
