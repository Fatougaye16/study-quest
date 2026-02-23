using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudyQuest.API.Configuration;
using StudyQuest.API.Data;
using StudyQuest.API.DTOs.Auth;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace StudyQuest.API.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IMemoryCache _cache;
    private readonly JwtSettings _jwtSettings;
    private readonly TwilioSettings _twilioSettings;
    private readonly ILogger<AuthService> _logger;
    private readonly IWebHostEnvironment _env;

    public AuthService(
        AppDbContext db,
        IMemoryCache cache,
        IOptions<JwtSettings> jwtSettings,
        IOptions<TwilioSettings> twilioSettings,
        ILogger<AuthService> logger,
        IWebHostEnvironment env)
    {
        _db = db;
        _cache = cache;
        _jwtSettings = jwtSettings.Value;
        _twilioSettings = twilioSettings.Value;
        _logger = logger;
        _env = env;
    }

    public async Task<bool> RequestOtpAsync(string phoneNumber)
    {
        // Rate limiting: max 3 OTP requests per phone per 10 minutes
        var rateLimitKey = $"otp_rate_{phoneNumber}";
        var attempts = _cache.GetOrCreate(rateLimitKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return 0;
        });

        if (attempts >= 3)
        {
            _logger.LogWarning("OTP rate limit exceeded for {PhoneNumber}", phoneNumber);
            return false;
        }

        _cache.Set(rateLimitKey, attempts + 1, TimeSpan.FromMinutes(10));

        // Generate 6-digit OTP
        var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

        // Store hashed OTP with 5-minute expiry
        var otpHash = HashOtp(otp);
        var otpKey = $"otp_{phoneNumber}";
        _cache.Set(otpKey, otpHash, TimeSpan.FromMinutes(5));

        // In development mode, log the OTP instead of sending SMS
        if (_env.IsDevelopment())
        {
            _logger.LogInformation("DEV OTP for {PhoneNumber}: {OTP}", phoneNumber, otp);
            return true;
        }

        // Send OTP via Twilio
        try
        {
            TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);

            await MessageResource.CreateAsync(
                to: new PhoneNumber(phoneNumber),
                from: new PhoneNumber(_twilioSettings.PhoneNumber),
                body: $"Your Study Quest verification code is: {otp}. It expires in 5 minutes."
            );

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send OTP to {PhoneNumber}", phoneNumber);
            return false;
        }
    }

    public async Task<AuthResponseDto?> VerifyOtpAsync(string phoneNumber, string otpCode)
    {
        var otpKey = $"otp_{phoneNumber}";

        if (!_cache.TryGetValue(otpKey, out string? storedHash))
        {
            _logger.LogWarning("No OTP found for {PhoneNumber}", phoneNumber);
            return null;
        }

        if (storedHash != HashOtp(otpCode))
        {
            _logger.LogWarning("Invalid OTP for {PhoneNumber}", phoneNumber);
            return null;
        }

        // Remove used OTP
        _cache.Remove(otpKey);

        // Find or create student
        var student = await _db.Students.FirstOrDefaultAsync(s => s.PhoneNumber == phoneNumber);

        if (student == null)
        {
            student = new Student
            {
                Id = Guid.NewGuid(),
                PhoneNumber = phoneNumber,
                Grade = 10, // Default grade, user updates later
                CreatedAt = DateTime.UtcNow
            };
            _db.Students.Add(student);
        }

        student.LastLoginAt = DateTime.UtcNow;

        // Generate tokens
        var accessToken = GenerateAccessToken(student);
        var refreshToken = GenerateRefreshToken();

        student.RefreshToken = refreshToken;
        student.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

        await _db.SaveChangesAsync();

        return new AuthResponseDto(
            AccessToken: accessToken,
            RefreshToken: refreshToken,
            ExpiresAt: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            Student: MapToDto(student)
        );
    }

    public async Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken)
    {
        var student = await _db.Students
            .FirstOrDefaultAsync(s => s.RefreshToken == refreshToken);

        if (student == null || student.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return null;
        }

        var newAccessToken = GenerateAccessToken(student);
        var newRefreshToken = GenerateRefreshToken();

        student.RefreshToken = newRefreshToken;
        student.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

        await _db.SaveChangesAsync();

        return new AuthResponseDto(
            AccessToken: newAccessToken,
            RefreshToken: newRefreshToken,
            ExpiresAt: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            Student: MapToDto(student)
        );
    }

    public async Task<bool> LogoutAsync(Guid studentId)
    {
        var student = await _db.Students.FindAsync(studentId);
        if (student == null) return false;

        student.RefreshToken = null;
        student.RefreshTokenExpiryTime = null;
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<StudentDto?> GetStudentAsync(Guid studentId)
    {
        var student = await _db.Students.FindAsync(studentId);
        return student == null ? null : MapToDto(student);
    }

    public async Task<StudentDto?> UpdateProfileAsync(Guid studentId, UpdateProfileDto dto)
    {
        var student = await _db.Students.FindAsync(studentId);
        if (student == null) return null;

        if (dto.FirstName is not null) student.FirstName = dto.FirstName;
        if (dto.LastName is not null) student.LastName = dto.LastName;
        if (dto.Grade.HasValue) student.Grade = dto.Grade.Value;
        if (dto.DailyGoalMinutes.HasValue) student.DailyGoalMinutes = dto.DailyGoalMinutes.Value;

        await _db.SaveChangesAsync();
        return MapToDto(student);
    }

    private string GenerateAccessToken(Student student)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, student.Id.ToString()),
            new Claim("phone", student.PhoneNumber),
            new Claim("grade", student.Grade.ToString()),
            new Claim(ClaimTypes.Name, $"{student.FirstName} {student.LastName}".Trim())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private static string HashOtp(string otp)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(otp));
        return Convert.ToBase64String(bytes);
    }

    private static StudentDto MapToDto(Student s) => new(
        Id: s.Id,
        PhoneNumber: s.PhoneNumber,
        FirstName: s.FirstName,
        LastName: s.LastName,
        Grade: s.Grade,
        DailyGoalMinutes: s.DailyGoalMinutes,
        CreatedAt: s.CreatedAt
    );
}
