using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Auth.Common;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Features.Auth.VerifyOtp;

public record VerifyOtpCommand(string PhoneNumber, string OtpCode) : IRequest<ErrorOr<AuthResponse>>;

internal sealed class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, ErrorOr<AuthResponse>>
{
    private readonly AppDbContext _dbContext;
    private readonly IOtpService _otpService;
    private readonly AuthTokenService _tokenService;

    public VerifyOtpCommandHandler(AppDbContext dbContext, IOtpService otpService, AuthTokenService tokenService)
    {
        _dbContext = dbContext;
        _otpService = otpService;
        _tokenService = tokenService;
    }

    public async Task<ErrorOr<AuthResponse>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        var verification = _otpService.VerifyOtp(request.PhoneNumber, request.OtpCode);
        if (verification.IsError)
            return verification.Errors;

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
                IsOtpEnabled = true,
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
