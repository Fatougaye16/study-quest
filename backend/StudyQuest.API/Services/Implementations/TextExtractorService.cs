using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using StudyQuest.API.Configuration;
using StudyQuest.API.Services.Interfaces;
using UglyToad.PdfPig;

namespace StudyQuest.API.Services.Implementations;

public class TextExtractorService : ITextExtractorService
{
    private readonly OpenAISettings _openAISettings;
    private readonly ILogger<TextExtractorService> _logger;

    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf", ".docx", ".txt", ".png", ".jpg", ".jpeg"
    };

    private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB

    public TextExtractorService(IOptions<OpenAISettings> openAISettings, ILogger<TextExtractorService> logger)
    {
        _openAISettings = openAISettings.Value;
        _logger = logger;
    }

    public async Task<string> ExtractAsync(Stream stream, string fileName, string contentType)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
            throw new InvalidOperationException($"Unsupported file type: {extension}. Allowed: {string.Join(", ", AllowedExtensions)}");

        if (stream.Length > MaxFileSize)
            throw new InvalidOperationException($"File too large. Maximum size is {MaxFileSize / (1024 * 1024)} MB.");

        return extension switch
        {
            ".pdf" => ExtractFromPdf(stream),
            ".docx" => ExtractFromDocx(stream),
            ".txt" => await ExtractFromTxt(stream),
            ".png" or ".jpg" or ".jpeg" => await ExtractFromImage(stream, fileName),
            _ => throw new InvalidOperationException($"Unsupported file type: {extension}")
        };
    }

    private static string ExtractFromPdf(Stream stream)
    {
        using var document = PdfDocument.Open(stream);
        var sb = new StringBuilder();

        foreach (var page in document.GetPages())
        {
            sb.AppendLine(page.Text);
        }

        var text = sb.ToString().Trim();
        if (string.IsNullOrWhiteSpace(text))
            throw new InvalidOperationException("Could not extract any text from the PDF. The file may be image-based — try uploading as an image instead.");

        return text;
    }

    private static string ExtractFromDocx(Stream stream)
    {
        using var doc = WordprocessingDocument.Open(stream, false);
        var body = doc.MainDocumentPart?.Document?.Body;
        if (body is null)
            throw new InvalidOperationException("Could not read the DOCX file. The document may be corrupted.");

        var sb = new StringBuilder();
        foreach (var paragraph in body.Elements<Paragraph>())
        {
            sb.AppendLine(paragraph.InnerText);
        }

        var text = sb.ToString().Trim();
        if (string.IsNullOrWhiteSpace(text))
            throw new InvalidOperationException("The DOCX file appears to be empty.");

        return text;
    }

    private static async Task<string> ExtractFromTxt(Stream stream)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8);
        var text = await reader.ReadToEndAsync();

        if (string.IsNullOrWhiteSpace(text))
            throw new InvalidOperationException("The text file is empty.");

        return text.Trim();
    }

    private async Task<string> ExtractFromImage(Stream stream, string fileName)
    {
        // Convert image to base64 for GPT-4o Vision
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        var imageBytes = ms.ToArray();
        var base64 = Convert.ToBase64String(imageBytes);

        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        var mimeType = extension switch
        {
            ".png" => "image/png",
            ".jpg" or ".jpeg" => "image/jpeg",
            _ => "image/jpeg"
        };

        var client = new ChatClient(model: "gpt-4o", apiKey: _openAISettings.ApiKey);

        var imageContent = ChatMessageContentPart.CreateImagePart(
            new BinaryData(imageBytes), mimeType);
        var textContent = ChatMessageContentPart.CreateTextPart(
            "Extract ALL text from this image exactly as written. " +
            "This is study material or an exam paper. Preserve the structure, " +
            "including headings, numbered lists, and question numbers. " +
            "If there are diagrams, describe them briefly in [brackets]. " +
            "Output only the extracted text, nothing else.");

        var messages = new List<ChatMessage>
        {
            new UserChatMessage(textContent, imageContent)
        };

        var options = new ChatCompletionOptions
        {
            Temperature = 0.1f,
            MaxOutputTokenCount = 4000
        };

        try
        {
            var completion = await client.CompleteChatAsync(messages, options);
            var text = completion.Value.Content[0].Text.Trim();

            if (string.IsNullOrWhiteSpace(text))
                throw new InvalidOperationException("Could not extract text from the image.");

            return text;
        }
        catch (Exception ex) when (ex is not InvalidOperationException)
        {
            _logger.LogError(ex, "OCR via GPT-4o Vision failed for {FileName}", fileName);
            throw new InvalidOperationException("Failed to extract text from the image. Please try again or use a clearer image.");
        }
    }
}
