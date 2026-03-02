namespace StudyQuest.API.Features.Timetable.Common;

public record TimetableEntryResponse(
    Guid Id, Guid SubjectId, string SubjectName, string SubjectColor,
    DayOfWeek DayOfWeek, TimeOnly StartTime, TimeOnly EndTime, string? Location);

public record CreateTimetableEntryRequest(
    Guid SubjectId, DayOfWeek DayOfWeek, TimeOnly StartTime, TimeOnly EndTime, string? Location);

public record UpdateTimetableEntryRequest(
    Guid SubjectId, DayOfWeek DayOfWeek, TimeOnly StartTime, TimeOnly EndTime, string? Location);
