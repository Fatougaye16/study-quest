using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Data;
using StudyQuest.API.DTOs.Subjects;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Services.Implementations;

public class SubjectService : ISubjectService
{
    private readonly AppDbContext _db;

    public SubjectService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<SubjectDto>> GetSubjectsByGradeAsync(int grade)
    {
        return await _db.Subjects
            .Where(s => s.Grade == grade)
            .Include(s => s.Topics)
            .OrderBy(s => s.Name)
            .Select(s => new SubjectDto(
                s.Id, s.Name, s.Grade, s.Description, s.Color, s.Topics.Count
            ))
            .ToListAsync();
    }

    public async Task<List<TopicDto>> GetTopicsBySubjectAsync(Guid subjectId)
    {
        return await _db.Topics
            .Where(t => t.SubjectId == subjectId)
            .Include(t => t.Notes)
            .Include(t => t.Questions)
            .OrderBy(t => t.Order)
            .Select(t => new TopicDto(
                t.Id, t.SubjectId, t.Name, t.Order, t.Description,
                t.Notes.Count, t.Questions.Count
            ))
            .ToListAsync();
    }

    public async Task<List<NoteDto>> GetNotesByTopicAsync(Guid topicId)
    {
        return await _db.Notes
            .Where(n => n.TopicId == topicId)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NoteDto(
                n.Id, n.TopicId, n.Title, n.Content, n.IsAIGenerated, n.CreatedAt
            ))
            .ToListAsync();
    }

    public async Task<NoteDto> CreateNoteAsync(Guid topicId, CreateNoteDto dto)
    {
        var topic = await _db.Topics.FindAsync(topicId)
            ?? throw new InvalidOperationException("Topic not found");

        var note = new Note
        {
            Id = Guid.NewGuid(),
            TopicId = topicId,
            Title = dto.Title,
            Content = dto.Content,
            IsAIGenerated = false
        };

        _db.Notes.Add(note);
        await _db.SaveChangesAsync();

        return new NoteDto(note.Id, note.TopicId, note.Title, note.Content, note.IsAIGenerated, note.CreatedAt);
    }

    public async Task<List<QuestionDto>> GetQuestionsByTopicAsync(Guid topicId)
    {
        return await _db.Questions
            .Where(q => q.TopicId == topicId)
            .OrderBy(q => q.Difficulty)
            .Select(q => new QuestionDto(
                q.Id, q.TopicId, q.QuestionText, q.AnswerText, q.Difficulty, q.IsAIGenerated
            ))
            .ToListAsync();
    }

    public async Task<QuestionDto> CreateQuestionAsync(Guid topicId, CreateQuestionDto dto)
    {
        var topic = await _db.Topics.FindAsync(topicId)
            ?? throw new InvalidOperationException("Topic not found");

        var question = new Question
        {
            Id = Guid.NewGuid(),
            TopicId = topicId,
            QuestionText = dto.QuestionText,
            AnswerText = dto.AnswerText,
            Difficulty = dto.Difficulty,
            IsAIGenerated = false
        };

        _db.Questions.Add(question);
        await _db.SaveChangesAsync();

        return new QuestionDto(
            question.Id, question.TopicId, question.QuestionText,
            question.AnswerText, question.Difficulty, question.IsAIGenerated
        );
    }
}
