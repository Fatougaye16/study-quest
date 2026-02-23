namespace StudyQuest.API.DTOs.Timetable;

public record TimetableEntryDto(
    Guid Id,
    Guid SubjectId,
    string SubjectName,
    string SubjectColor,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    string? Location
);

public record CreateTimetableEntryDto(
    Guid SubjectId,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    string? Location
);

public record UpdateTimetableEntryDto(
    Guid SubjectId,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    string? Location
);
