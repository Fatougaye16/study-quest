using System.Security.Claims;

namespace StudyQuest.API.Common;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetStudentId(this ClaimsPrincipal principal, out Guid studentId)
    {
        var value = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out studentId);
    }
}
