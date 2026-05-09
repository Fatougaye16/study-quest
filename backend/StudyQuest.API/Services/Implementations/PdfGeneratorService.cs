using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Services.Implementations;

public class PdfGeneratorService : IPdfGeneratorService
{
    private const string BrandColor = "#BF4A0A";

    public byte[] GeneratePastPaperPdf(PastPaper paper, string subjectName, List<PastQuestion> questions)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Column(col =>
                {
                    col.Item().Text("Study Quest").FontSize(10).FontColor(BrandColor).Bold();
                    col.Item().PaddingBottom(5).LineHorizontal(1).LineColor(BrandColor);
                    col.Item().Text($"{subjectName} — {paper.ExamType} {paper.Year} Paper {paper.PaperNumber}")
                        .FontSize(16).Bold();
                    col.Item().Text(paper.Title).FontSize(12).FontColor(Colors.Grey.Darken1);
                    col.Item().PaddingTop(5).PaddingBottom(10)
                        .Text($"{questions.Count} Questions").FontSize(10).FontColor(Colors.Grey.Medium);
                });

                page.Content().Column(col =>
                {
                    // Questions section
                    col.Item().PaddingBottom(10).Text("QUESTIONS").FontSize(13).Bold().FontColor(BrandColor);

                    foreach (var q in questions.OrderBy(q => q.QuestionNumber))
                    {
                        col.Item().PaddingBottom(8).Row(row =>
                        {
                            row.ConstantItem(30).Text($"{q.QuestionNumber}.").Bold();
                            row.RelativeItem().Column(qCol =>
                            {
                                qCol.Item().Text(q.QuestionText);
                                if (q.Marks.HasValue)
                                    qCol.Item().Text($"[{q.Marks} marks]").FontSize(9).Italic().FontColor(Colors.Grey.Darken1);
                            });
                        });
                    }

                    // Answers section
                    col.Item().PaddingTop(20).PaddingBottom(10)
                        .Text("ANSWERS").FontSize(13).Bold().FontColor(BrandColor);

                    foreach (var q in questions.Where(q => !string.IsNullOrWhiteSpace(q.AnswerText)).OrderBy(q => q.QuestionNumber))
                    {
                        col.Item().PaddingBottom(6).Row(row =>
                        {
                            row.ConstantItem(30).Text($"{q.QuestionNumber}.").Bold();
                            row.RelativeItem().Text(q.AnswerText!);
                        });
                    }
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                    x.Span(" of ");
                    x.TotalPages();
                });
            });
        }).GeneratePdf();
    }

    public byte[] GenerateNotesPdf(string subjectName, string topicName, List<Note> notes)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Column(col =>
                {
                    col.Item().Text("Study Quest").FontSize(10).FontColor(BrandColor).Bold();
                    col.Item().PaddingBottom(5).LineHorizontal(1).LineColor(BrandColor);
                    col.Item().Text($"{subjectName} — {topicName}").FontSize(16).Bold();
                    col.Item().Text($"{notes.Count} Notes").FontSize(10).FontColor(Colors.Grey.Medium);
                });

                page.Content().Column(col =>
                {
                    foreach (var note in notes.OrderByDescending(n => n.IsOfficial).ThenBy(n => n.CreatedAt))
                    {
                        col.Item().PaddingBottom(15).Column(noteCol =>
                        {
                            var label = note.IsOfficial ? " [Official]" : "";
                            noteCol.Item().Text($"{note.Title}{label}").FontSize(13).Bold().FontColor(BrandColor);
                            noteCol.Item().PaddingTop(4).Text(note.Content);
                            noteCol.Item().PaddingTop(6).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten2);
                        });
                    }
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                    x.Span(" of ");
                    x.TotalPages();
                });
            });
        }).GeneratePdf();
    }

    public byte[] GenerateFlashcardsPdf(string topicName, List<Flashcard> flashcards)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Column(col =>
                {
                    col.Item().Text("Study Quest").FontSize(10).FontColor(BrandColor).Bold();
                    col.Item().PaddingBottom(5).LineHorizontal(1).LineColor(BrandColor);
                    col.Item().Text($"Flashcards — {topicName}").FontSize(16).Bold();
                    col.Item().Text($"{flashcards.Count} Cards").FontSize(10).FontColor(Colors.Grey.Medium);
                });

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.ConstantColumn(30);
                        cols.RelativeColumn();
                        cols.RelativeColumn();
                    });

                    // Header
                    table.Header(header =>
                    {
                        header.Cell().PaddingBottom(8).Text("#").Bold();
                        header.Cell().PaddingBottom(8).Text("Front (Question)").Bold().FontColor(BrandColor);
                        header.Cell().PaddingBottom(8).Text("Back (Answer)").Bold().FontColor(BrandColor);
                    });

                    var idx = 1;
                    foreach (var fc in flashcards)
                    {
                        table.Cell().PaddingBottom(6).Text($"{idx}.");
                        table.Cell().PaddingBottom(6).Text(fc.Front);
                        table.Cell().PaddingBottom(6).Text(fc.Back);
                        idx++;
                    }
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                    x.Span(" of ");
                    x.TotalPages();
                });
            });
        }).GeneratePdf();
    }

    public byte[] GenerateStudyPlanPdf(StudyPlan plan, string subjectName, List<StudyPlanItem> items)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Column(col =>
                {
                    col.Item().Text("Study Quest").FontSize(10).FontColor(BrandColor).Bold();
                    col.Item().PaddingBottom(5).LineHorizontal(1).LineColor(BrandColor);
                    col.Item().Text(plan.Title).FontSize(16).Bold();
                    col.Item().Text($"{subjectName} — {plan.StartDate:d MMM yyyy} to {plan.EndDate:d MMM yyyy}")
                        .FontSize(12).FontColor(Colors.Grey.Darken1);
                    var completed = items.Count(i => i.IsCompleted);
                    col.Item().Text($"{completed}/{items.Count} completed").FontSize(10).FontColor(Colors.Grey.Medium);
                });

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.RelativeColumn(2); // Date
                        cols.RelativeColumn(3); // Topic
                        cols.ConstantColumn(60); // Duration
                        cols.ConstantColumn(60); // Status
                    });

                    table.Header(header =>
                    {
                        header.Cell().PaddingBottom(8).Text("Date").Bold();
                        header.Cell().PaddingBottom(8).Text("Topic").Bold();
                        header.Cell().PaddingBottom(8).Text("Duration").Bold();
                        header.Cell().PaddingBottom(8).Text("Status").Bold();
                    });

                    foreach (var item in items.OrderBy(i => i.ScheduledDate))
                    {
                        table.Cell().PaddingBottom(4).Text(item.ScheduledDate.ToString("ddd d MMM"));
                        table.Cell().PaddingBottom(4).Text(item.Topic?.Name ?? "—");
                        table.Cell().PaddingBottom(4).Text($"{item.DurationMinutes} min");
                        table.Cell().PaddingBottom(4).Text(item.IsCompleted ? "Done" : "Pending")
                            .FontColor(item.IsCompleted ? Colors.Green.Darken1 : Colors.Grey.Medium);
                    }
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                    x.Span(" of ");
                    x.TotalPages();
                });
            });
        }).GeneratePdf();
    }
}
