using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using StudyQuest.API.Configuration;
using StudyQuest.API.Data;
using StudyQuest.API.DTOs.AI;
using StudyQuest.API.DTOs.StudyPlans;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Services.Implementations;

public class AIService : IAIService
{
    private readonly AppDbContext _db;
    private readonly OpenAISettings _settings;
    private readonly IMemoryCache _cache;
    private readonly ILogger<AIService> _logger;

    public AIService(
        AppDbContext db,
        IOptions<OpenAISettings> settings,
        IMemoryCache cache,
        ILogger<AIService> logger)
    {
        _db = db;
        _settings = settings.Value;
        _cache = cache;
        _logger = logger;
    }

    public async Task<SummarizeResponseDto> SummarizeAsync(Guid studentId, SummarizeRequestDto request)
    {
        var student = await _db.Students.FindAsync(studentId)
            ?? throw new InvalidOperationException("Student not found");

        var grade = request.Grade ?? student.Grade;
        var topic = await _db.Topics.Include(t => t.Notes).FirstOrDefaultAsync(t => t.Id == request.TopicId)
            ?? throw new InvalidOperationException("Topic not found");

        var content = request.Content ?? string.Join("\n\n", topic.Notes.Select(n => $"## {n.Title}\n{n.Content}"));

        if (string.IsNullOrWhiteSpace(content))
            throw new InvalidOperationException("No content available to summarize");

        var cacheKey = $"ai_summary_{request.TopicId}_{grade}_{content.GetHashCode()}";
        if (_cache.TryGetValue(cacheKey, out SummarizeResponseDto? cached) && cached != null)
            return cached;

        var systemPrompt = $$"""
            You are an educational assistant for South African high school students.
            Summarize the following study material for a Grade {{grade}} student into clear, concise bullet points.
            
            Return your response as JSON with this exact format:
            {
                "summary": "A brief 2-3 sentence overview",
                "keyPoints": ["Point 1", "Point 2", "Point 3", ...]
            }
            
            Make the summary easy to understand and highlight the most important concepts.
            Only output valid JSON, nothing else.
            """;

        var response = await CallOpenAIAsync(systemPrompt, $"Topic: {topic.Name}\n\nContent:\n{content}", _settings.Model);
        var result = JsonSerializer.Deserialize<SummarizeResponseDto>(response, JsonOptions)
            ?? new SummarizeResponseDto("Summary not available", []);

        _cache.Set(cacheKey, result, TimeSpan.FromHours(24));
        return result;
    }

    public async Task<FlashcardResponseDto> GenerateFlashcardsAsync(Guid studentId, FlashcardRequestDto request)
    {
        var student = await _db.Students.FindAsync(studentId)
            ?? throw new InvalidOperationException("Student not found");

        var topic = await _db.Topics.Include(t => t.Notes).FirstOrDefaultAsync(t => t.Id == request.TopicId)
            ?? throw new InvalidOperationException("Topic not found");

        var subject = await _db.Subjects.FindAsync(topic.SubjectId);
        var content = request.Content ?? string.Join("\n\n", topic.Notes.Select(n => $"## {n.Title}\n{n.Content}"));

        var cacheKey = $"ai_flashcards_{request.TopicId}_{request.Count}_{student.Grade}";
        if (_cache.TryGetValue(cacheKey, out FlashcardResponseDto? cached) && cached != null)
            return cached;

        var systemPrompt = $$"""
            You are an educational assistant for South African Grade {{student.Grade}} students studying {{subject?.Name ?? "this subject"}}.
            Generate exactly {{request.Count}} flashcards from the given content.
            
            Return your response as JSON with this exact format:
            {
                "flashcards": [
                    { "front": "Question or concept", "back": "Answer or explanation" },
                    ...
                ]
            }
            
            Make flashcards appropriate for Grade {{student.Grade}} level.
            Cover the most important concepts, definitions, formulas, and key facts.
            Only output valid JSON, nothing else.
            """;

        var userContent = string.IsNullOrWhiteSpace(content)
            ? $"Generate flashcards about the topic: {topic.Name}"
            : $"Topic: {topic.Name}\n\nContent:\n{content}";

        var response = await CallOpenAIAsync(systemPrompt, userContent, _settings.Model);
        var result = JsonSerializer.Deserialize<FlashcardResponseDto>(response, JsonOptions)
            ?? new FlashcardResponseDto([]);

        // Optionally save flashcards to DB
        foreach (var fc in result.Flashcards)
        {
            _db.Flashcards.Add(new Flashcard
            {
                Id = Guid.NewGuid(),
                TopicId = request.TopicId,
                StudentId = studentId,
                Front = fc.Front,
                Back = fc.Back,
                IsAIGenerated = true
            });
        }
        await _db.SaveChangesAsync();

        _cache.Set(cacheKey, result, TimeSpan.FromHours(12));
        return result;
    }

    public async Task<QuizResponseDto> GenerateQuizAsync(Guid studentId, QuizRequestDto request)
    {
        var student = await _db.Students.FindAsync(studentId)
            ?? throw new InvalidOperationException("Student not found");

        var topic = await _db.Topics.Include(t => t.Notes).Include(t => t.Questions)
            .FirstOrDefaultAsync(t => t.Id == request.TopicId)
            ?? throw new InvalidOperationException("Topic not found");

        var subject = await _db.Subjects.FindAsync(topic.SubjectId);

        var difficultyText = request.Difficulty switch
        {
            1 => "easy (basic recall and understanding)",
            2 => "medium (application and analysis)",
            3 => "hard (synthesis and evaluation)",
            _ => "mixed (varying difficulty levels)"
        };

        var notesContent = string.Join("\n", topic.Notes.Select(n => n.Content));
        var existingQA = string.Join("\n", topic.Questions.Select(q => $"Q: {q.QuestionText} A: {q.AnswerText}"));

        var systemPrompt = $$"""
            You are an educational quiz generator for South African Grade {{student.Grade}} students studying {{subject?.Name ?? "this subject"}}.
            Generate exactly {{request.QuestionCount}} multiple-choice questions about "{{topic.Name}}".
            Difficulty level: {{difficultyText}}
            
            Return your response as JSON with this exact format:
            {
                "questions": [
                    {
                        "question": "The question text",
                        "options": ["Option A", "Option B", "Option C", "Option D"],
                        "correctAnswer": "The correct option text (must match one of the options exactly)",
                        "explanation": "Brief explanation of why this is correct"
                    },
                    ...
                ]
            }
            
            Ensure questions test understanding, not just memorization.
            Only output valid JSON, nothing else.
            """;

        var userContent = $"Topic: {topic.Name}";
        if (!string.IsNullOrWhiteSpace(notesContent))
            userContent += $"\n\nStudy Material:\n{notesContent}";
        if (!string.IsNullOrWhiteSpace(existingQA))
            userContent += $"\n\nReference Q&A:\n{existingQA}";

        var response = await CallOpenAIAsync(systemPrompt, userContent, _settings.Model);
        var result = JsonSerializer.Deserialize<QuizResponseDto>(response, JsonOptions)
            ?? new QuizResponseDto([]);

        return result;
    }

    public async Task<ExplainResponseDto> ExplainTopicAsync(Guid studentId, ExplainRequestDto request)
    {
        var student = await _db.Students.FindAsync(studentId)
            ?? throw new InvalidOperationException("Student not found");

        var grade = request.Grade ?? student.Grade;
        var topic = await _db.Topics.Include(t => t.Subject).FirstOrDefaultAsync(t => t.Id == request.TopicId)
            ?? throw new InvalidOperationException("Topic not found");

        var cacheKey = $"ai_explain_{request.TopicId}_{grade}_{request.SpecificQuestion?.GetHashCode() ?? 0}";
        if (_cache.TryGetValue(cacheKey, out ExplainResponseDto? cached) && cached != null)
            return cached;

        var systemPrompt = $$"""
            You are a patient, encouraging teacher explaining {{topic.Subject.Name}} concepts to a South African Grade {{grade}} student.
            Use simple language, real-world examples, and analogies they can relate to.
            
            Return your response as JSON with this exact format:
            {
                "explanation": "A clear, detailed explanation using simple language",
                "examples": ["Example 1 with real-world context", "Example 2", ...],
                "keyTakeaways": ["Key point 1", "Key point 2", ...]
            }
            
            Be encouraging and make learning feel achievable.
            Only output valid JSON, nothing else.
            """;

        var userContent = string.IsNullOrWhiteSpace(request.SpecificQuestion)
            ? $"Please explain this topic: {topic.Name}\nDescription: {topic.Description}"
            : $"Topic: {topic.Name}\nStudent's question: {request.SpecificQuestion}";

        // Use the more powerful model for explanations
        var response = await CallOpenAIAsync(systemPrompt, userContent, _settings.ExplanationModel);
        var result = JsonSerializer.Deserialize<ExplainResponseDto>(response, JsonOptions)
            ?? new ExplainResponseDto("Explanation not available", [], []);

        _cache.Set(cacheKey, result, TimeSpan.FromHours(24));
        return result;
    }

    public async Task<StudyPlanDto> GenerateStudyPlanAsync(Guid studentId, AIStudyPlanRequestDto request)
    {
        var student = await _db.Students.FindAsync(studentId)
            ?? throw new InvalidOperationException("Student not found");

        var subject = await _db.Subjects.Include(s => s.Topics)
            .FirstOrDefaultAsync(s => s.Id == request.SubjectId)
            ?? throw new InvalidOperationException("Subject not found");

        var topics = request.TopicIds?.Count > 0
            ? subject.Topics.Where(t => request.TopicIds.Contains(t.Id)).OrderBy(t => t.Order).ToList()
            : subject.Topics.OrderBy(t => t.Order).ToList();

        if (topics.Count == 0)
            throw new InvalidOperationException("No topics found for study plan");

        var topicList = string.Join("\n", topics.Select((t, i) => $"{i + 1}. {t.Name}"));

        var systemPrompt = $$"""
            You are a study plan generator for a South African Grade {{student.Grade}} student studying {{subject.Name}}.
            Create a {{request.DurationDays}}-day study plan covering the listed topics.
            
            Return your response as JSON with this exact format:
            {
                "title": "Study plan title",
                "items": [
                    {
                        "topicIndex": 0,
                        "day": 1,
                        "durationMinutes": 45
                    },
                    ...
                ]
            }
            
            Rules:
            - topicIndex is 0-based, matching the topic list order
            - Spread topics evenly across days
            - Suggest 30-60 minute study sessions
            - Include review days for previously covered topics
            - Plan rest days if duration > 7 days
            Only output valid JSON, nothing else.
            """;

        var response = await CallOpenAIAsync(systemPrompt, $"Topics to cover:\n{topicList}", _settings.Model);

        // Parse AI response and create study plan
        using var doc = JsonDocument.Parse(response);
        var root = doc.RootElement;
        var title = root.GetProperty("title").GetString() ?? $"{subject.Name} Study Plan";

        var startDate = DateTime.UtcNow.Date;
        var endDate = startDate.AddDays(request.DurationDays);

        var studyPlan = new StudyPlan
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            SubjectId = request.SubjectId,
            Title = title,
            StartDate = startDate,
            EndDate = endDate,
            IsAIGenerated = true
        };

        _db.StudyPlans.Add(studyPlan);

        var items = new List<StudyPlanItem>();
        foreach (var item in root.GetProperty("items").EnumerateArray())
        {
            var topicIndex = item.GetProperty("topicIndex").GetInt32();
            var day = item.GetProperty("day").GetInt32();
            var duration = item.GetProperty("durationMinutes").GetInt32();

            if (topicIndex >= 0 && topicIndex < topics.Count)
            {
                var planItem = new StudyPlanItem
                {
                    Id = Guid.NewGuid(),
                    StudyPlanId = studyPlan.Id,
                    TopicId = topics[topicIndex].Id,
                    ScheduledDate = startDate.AddDays(day - 1),
                    DurationMinutes = duration,
                    IsCompleted = false
                };
                items.Add(planItem);
                _db.StudyPlanItems.Add(planItem);
            }
        }

        await _db.SaveChangesAsync();

        return new StudyPlanDto(
            Id: studyPlan.Id,
            SubjectId: subject.Id,
            SubjectName: subject.Name,
            Title: title,
            StartDate: startDate,
            EndDate: endDate,
            IsAIGenerated: true,
            CreatedAt: studyPlan.CreatedAt,
            Items: items.Select(i => new StudyPlanItemDto(
                Id: i.Id,
                TopicId: i.TopicId,
                TopicName: topics.First(t => t.Id == i.TopicId).Name,
                ScheduledDate: i.ScheduledDate,
                DurationMinutes: i.DurationMinutes,
                IsCompleted: false,
                CompletedAt: null
            )).ToList(),
            CompletionPercentage: 0
        );
    }

    private async Task<string> CallOpenAIAsync(string systemPrompt, string userMessage, string model)
    {
        var client = new ChatClient(model: model, apiKey: _settings.ApiKey);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(userMessage)
        };

        var options = new ChatCompletionOptions
        {
            Temperature = 0.7f,
            MaxOutputTokenCount = 4000
        };

        try
        {
            var completion = await client.CompleteChatAsync(messages, options);
            var content = completion.Value.Content[0].Text;

            // Strip markdown code fences if present
            content = content.Trim();
            if (content.StartsWith("```json", StringComparison.OrdinalIgnoreCase))
                content = content[7..];
            else if (content.StartsWith("```"))
                content = content[3..];
            if (content.EndsWith("```"))
                content = content[..^3];

            return content.Trim();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OpenAI API call failed for model {Model}", model);
            throw new InvalidOperationException("AI service is temporarily unavailable. Please try again later.", ex);
        }
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
