using System.Security.Cryptography;
using System.Text;

namespace StudyQuest.API.Features.Auth.Common;

public static class OtpHelper
{
    public static string BuildCacheKey(string phoneNumber) => $"otp_{phoneNumber}";

    public static string Hash(string value)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToBase64String(bytes);
    }
}
