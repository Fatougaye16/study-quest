using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using StudyQuest.API.Configuration;

namespace StudyQuest.API.Features.AI.Common;

public class OpenAIClient
{
    private readonly OpenAISettings _settings;
    private readonly IMemoryCache _cache;
    private readonly ILogger<OpenAIClient> _logger;

    public OpenAIClient(IOptions<OpenAISettings> settings, IMemoryCache cache, ILogger<OpenAIClient> logger)
    {
        _settings = settings.Value;
        _cache = cache;
        _logger = logger;
    }

    public string Model => _settings.Model;
    public string ExplanationModel => _settings.ExplanationModel;

    public async Task<string> CallAsync(string systemPrompt, string userMessage, string model, float temperature = 0.7f)
    {
        var client = new ChatClient(model: model, apiKey: _settings.ApiKey);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(userMessage)
        };

        var options = new ChatCompletionOptions
        {
            Temperature = temperature,
            MaxOutputTokenCount = 4000,
            ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
        };

        return await ExecuteWithRetry(client, messages, options);
    }

    private async Task<string> ExecuteWithRetry(ChatClient client, List<ChatMessage> messages, ChatCompletionOptions options, int maxRetries = 1)
    {
        for (var attempt = 0; attempt <= maxRetries; attempt++)
        {
            var completion = await client.CompleteChatAsync(messages, options);
            var content = StripMarkdownWrapper(completion.Value.Content[0].Text.Trim());

            if (attempt < maxRetries)
            {
                try
                {
                    using var doc = JsonDocument.Parse(content);
                    return content;
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(ex, "AI returned malformed JSON on attempt {Attempt}, retrying", attempt + 1);
                    options.Temperature = Math.Max(0.1f, (options.Temperature ?? 0.7f) - 0.2f);
                }
            }
            else
            {
                return content;
            }
        }
        return "{}";
    }

    private static string StripMarkdownWrapper(string content)
    {
        if (content.StartsWith("```json", StringComparison.OrdinalIgnoreCase))
            content = content[7..];
        else if (content.StartsWith("```"))
            content = content[3..];
        if (content.EndsWith("```"))
            content = content[..^3];
        return content.Trim();
    }

    public T? TryGetCached<T>(string key) where T : class
    {
        _cache.TryGetValue(key, out T? value);
        return value;
    }

    public void SetCache<T>(string key, T value, TimeSpan duration) => _cache.Set(key, value, duration);

    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
