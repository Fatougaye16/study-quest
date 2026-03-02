using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Progress.Common;

namespace StudyQuest.API.Features.Progress.GetAchievements;

public record GetAchievementsQuery(Guid StudentId) : IRequest<ErrorOr<List<AchievementDefinitionResponse>>>;

internal sealed class GetAchievementsQueryHandler : IRequestHandler<GetAchievementsQuery, ErrorOr<List<AchievementDefinitionResponse>>>
{
    private readonly AppDbContext _db;

    private static readonly List<AchievementDef> Definitions =
    [
        new("first_steps", "First Steps", "Complete your first study session", "🎯", 50),
        new("getting_organized", "Getting Organized", "Add your first timetable entry", "📅", 30),
        new("study_planner", "Study Planner", "Complete a study plan item", "📋", 40),
        new("dedicated_student", "Dedicated Student", "Study for 10 hours total", "📚", 100),
        new("study_marathon", "Study Marathon", "Study for 50 hours total", "🏃", 250),
        new("week_warrior", "Week Warrior", "Maintain a 7-day streak", "🔥", 150),
        new("knowledge_seeker", "Knowledge Seeker", "Complete 5 study sessions", "🔍", 75),
        new("course_master", "Course Master", "Enroll in 5 subjects", "🎓", 100),
        new("quiz_champion", "Quiz Champion", "Score 100% on a quiz", "🏆", 200),
        new("flashcard_fan", "Flashcard Fan", "Generate 50 flashcards", "🃏", 75),
        new("consistency_king", "Consistency King", "Maintain a 30-day streak", "👑", 500),
        new("ai_explorer", "AI Explorer", "Use AI features 10 times", "🤖", 100),
    ];

    public GetAchievementsQueryHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<List<AchievementDefinitionResponse>>> Handle(GetAchievementsQuery request, CancellationToken ct)
    {
        var unlocked = await _db.Achievements
            .Where(a => a.StudentId == request.StudentId)
            .ToListAsync(ct);

        var results = Definitions.Select(def =>
        {
            var match = unlocked.FirstOrDefault(a => a.Type == def.Type);
            return new AchievementDefinitionResponse(
                def.Type, def.Title, def.Description, def.Icon, def.XPReward,
                match is not null, match?.UnlockedAt);
        }).ToList();

        return results;
    }

    private record AchievementDef(string Type, string Title, string Description, string Icon, int XPReward);
}
