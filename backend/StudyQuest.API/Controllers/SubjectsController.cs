using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyQuest.API.DTOs.Subjects;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Controllers;

[Route("api/[controller]")]
[Authorize]
public class SubjectsController : BaseApiController
{
    private readonly ISubjectService _subjectService;

    public SubjectsController(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    /// <summary>
    /// Get all subjects for a specific grade (10, 11, or 12).
    /// </summary>
    [HttpGet]
    [ProducesResponseType<List<SubjectDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSubjects([FromQuery] int? grade)
    {
        var studentGrade = grade ?? GetStudentGrade();
        var subjects = await _subjectService.GetSubjectsByGradeAsync(studentGrade);
        return Ok(subjects);
    }

    /// <summary>
    /// Get all topics for a subject.
    /// </summary>
    [HttpGet("{subjectId:guid}/topics")]
    [ProducesResponseType<List<TopicDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopics(Guid subjectId)
    {
        var topics = await _subjectService.GetTopicsBySubjectAsync(subjectId);
        return Ok(topics);
    }

    /// <summary>
    /// Get all notes for a topic.
    /// </summary>
    [HttpGet("topics/{topicId:guid}/notes")]
    [ProducesResponseType<List<NoteDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotes(Guid topicId)
    {
        var notes = await _subjectService.GetNotesByTopicAsync(topicId);
        return Ok(notes);
    }

    /// <summary>
    /// Create a new note for a topic (admin/teacher).
    /// </summary>
    [HttpPost("topics/{topicId:guid}/notes")]
    [ProducesResponseType<NoteDto>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateNote(Guid topicId, [FromBody] CreateNoteDto dto)
    {
        var note = await _subjectService.CreateNoteAsync(topicId, dto);
        return CreatedAtAction(nameof(GetNotes), new { topicId }, note);
    }

    /// <summary>
    /// Get all questions for a topic.
    /// </summary>
    [HttpGet("topics/{topicId:guid}/questions")]
    [ProducesResponseType<List<QuestionDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetQuestions(Guid topicId)
    {
        var questions = await _subjectService.GetQuestionsByTopicAsync(topicId);
        return Ok(questions);
    }

    /// <summary>
    /// Create a question for a topic (admin/teacher).
    /// </summary>
    [HttpPost("topics/{topicId:guid}/questions")]
    [ProducesResponseType<QuestionDto>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateQuestion(Guid topicId, [FromBody] CreateQuestionDto dto)
    {
        var question = await _subjectService.CreateQuestionAsync(topicId, dto);
        return CreatedAtAction(nameof(GetQuestions), new { topicId }, question);
    }
}
