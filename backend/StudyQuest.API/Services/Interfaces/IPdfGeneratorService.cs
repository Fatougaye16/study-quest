using StudyQuest.API.Models;

namespace StudyQuest.API.Services.Interfaces;

public interface IPdfGeneratorService
{
    byte[] GeneratePastPaperPdf(PastPaper paper, string subjectName, List<PastQuestion> questions);
    byte[] GenerateNotesPdf(string subjectName, string topicName, List<Note> notes);
    byte[] GenerateFlashcardsPdf(string topicName, List<Flashcard> flashcards);
    byte[] GenerateStudyPlanPdf(StudyPlan plan, string subjectName, List<StudyPlanItem> items);
}
