namespace StudyQuest.API.Features.AI.Common;

/// <summary>
/// Shared WASSCE-aligned prompt fragments used across all AI features.
/// </summary>
public static class WASSCEPromptContext
{
    public const string BaseContext =
        "You are an educational assistant for Senior Secondary School students in The Gambia " +
        "preparing for the WASSCE (West African Senior School Certificate Examination) administered by WAEC. " +
        "All content you generate must be strictly aligned with the official WASSCE/WAEC syllabus.";

    public const string ExamFormatGuidance =
        "WASSCE exam papers are typically structured as: " +
        "Paper 1 (Objectives) — multiple-choice questions testing recall and comprehension; " +
        "Paper 2 (Theory/Essay) — structured and essay questions testing application, analysis, and evaluation; " +
        "Paper 3 (Practical/Alternative to Practical) — hands-on or scenario-based questions where applicable. " +
        "Tailor your output to reflect these exam formats and the way WAEC frames questions.";

    public const string ExamTipsGuidance =
        "Include practical WASSCE exam tips: common pitfalls to avoid, " +
        "frequently tested areas, how marks are typically allocated, " +
        "and strategies for answering WAEC-style questions effectively.";

    public static string DifficultyMapping(int? difficulty) => difficulty switch
    {
        1 => "easy — WASSCE Paper 1 (Objectives) level: basic recall, definitions, and straightforward comprehension",
        2 => "medium — WASSCE Paper 2 (Theory) structured question level: application, worked examples, and short explanations",
        3 => "hard — WASSCE Paper 2 (Theory) essay level: analysis, evaluation, multi-step reasoning, and extended responses",
        _ => "mixed — varying difficulty covering Paper 1 objectives through Paper 2 essay-level questions"
    };
}
