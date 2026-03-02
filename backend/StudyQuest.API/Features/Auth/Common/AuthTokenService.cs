using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudyQuest.API.Configuration;
using StudyQuest.API.Models;

namespace StudyQuest.API.Features.Auth.Common;

public class AuthTokenService
{
    private readonly JwtSettings _jwtSettings;

    public AuthTokenService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public AuthResponse GenerateTokens(Student student)
    {
        var accessToken = GenerateAccessToken(student);
        var refreshToken = GenerateRefreshToken();

        student.RefreshToken = refreshToken;
        student.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

        return new AuthResponse(
            AccessToken: accessToken,
            RefreshToken: refreshToken,
            ExpiresAt: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            Student: student.ToResponse());
    }

    private string GenerateAccessToken(Student student)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, student.Id.ToString()),
            new("phone", student.PhoneNumber),
            new("grade", student.Grade.ToString())
        };

        if (!string.IsNullOrWhiteSpace(student.FirstName) || !string.IsNullOrWhiteSpace(student.LastName))
        {
            claims.Add(new Claim(ClaimTypes.Name, $"{student.FirstName} {student.LastName}".Trim()));
        }

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}
