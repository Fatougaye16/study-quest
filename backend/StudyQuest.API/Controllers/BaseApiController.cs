using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace StudyQuest.API.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected Guid GetStudentId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claim) || !Guid.TryParse(claim, out var studentId))
            throw new UnauthorizedAccessException("Invalid token");
        return studentId;
    }

    protected int GetStudentGrade()
    {
        var claim = User.FindFirst("grade")?.Value;
        return int.TryParse(claim, out var grade) ? grade : 10;
    }
}
