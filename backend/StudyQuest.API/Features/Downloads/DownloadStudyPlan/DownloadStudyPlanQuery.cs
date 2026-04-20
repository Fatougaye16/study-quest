using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ErrorOr;
using StudyQuest.API.Data;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Features.Downloads.DownloadStudyPlan;

public record DownloadStudyPlanQuery(Guid PlanId, Guid StudentId) : IRequest<ErrorOr<DownloadResult>>;

public record DownloadResult(byte[] PdfData, string FileName);

public class DownloadStudyPlanHandler(AppDbContext db, IPdfGeneratorService pdf) : IRequestHandler<DownloadStudyPlanQuery, ErrorOr<DownloadResult>>
{
    public async Task<ErrorOr<DownloadResult>> Handle(DownloadStudyPlanQuery request, CancellationToken ct)
    {
        var cached = await db.CachedDownloads
            .FirstOrDefaultAsync(c => c.ContentType == DownloadContentType.StudyPlan && c.SourceId == request.PlanId, ct);

        if (cached is not null)
            return new DownloadResult(cached.PdfData, cached.FileName);

        var plan = await db.StudyPlans
            .Include(p => p.Subject)
            .FirstOrDefaultAsync(p => p.Id == request.PlanId && p.StudentId == request.StudentId, ct);

        if (plan is null)
            return Error.NotFound("StudyPlan.NotFound", "Study plan not found.");

        var items = await db.StudyPlanItems
            .Include(i => i.Topic)
            .Where(i => i.StudyPlanId == request.PlanId)
            .OrderBy(i => i.ScheduledDate)
            .ToListAsync(ct);

        var pdfBytes = pdf.GenerateStudyPlanPdf(plan, plan.Subject.Name, items);
        var fileName = $"{plan.Title.Replace(' ', '_')}.pdf";
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes($"{plan.Id}|{items.Count}|{items.Count(i => i.IsCompleted)}")));

        db.CachedDownloads.Add(new CachedDownload
        {
            ContentType = DownloadContentType.StudyPlan,
            SourceId = request.PlanId,
            FileName = fileName,
            PdfData = pdfBytes,
            ContentHash = hash,
            FileSizeBytes = pdfBytes.Length
        });
        await db.SaveChangesAsync(ct);

        return new DownloadResult(pdfBytes, fileName);
    }
}
