using ErrorOr;

namespace StudyQuest.API.Features.Timetable.Common;

public static class TimetableErrors
{
    public static Error SubjectNotFound => Error.NotFound(
        code: "Timetable.SubjectNotFound",
        description: "The requested subject could not be found.");

    public static Error EntryNotFound => Error.NotFound(
        code: "Timetable.EntryNotFound",
        description: "The timetable entry could not be found.");
}
