using StudyQuest.API.Models;

namespace StudyQuest.API.Features.Auth.Common;

public static class StudentMappings
{
    public static StudentResponse ToResponse(this Student student) => new(
        Id: student.Id,
        PhoneNumber: student.PhoneNumber,
        FirstName: student.FirstName,
        LastName: student.LastName,
        Grade: student.Grade,
        DailyGoalMinutes: student.DailyGoalMinutes,
        CreatedAt: student.CreatedAt);
}
