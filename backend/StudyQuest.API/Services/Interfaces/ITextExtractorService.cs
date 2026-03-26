namespace StudyQuest.API.Services.Interfaces;

public interface ITextExtractorService
{
    Task<string> ExtractAsync(Stream stream, string fileName, string contentType);
}
