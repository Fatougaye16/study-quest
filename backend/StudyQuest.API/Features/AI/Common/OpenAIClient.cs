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

    public async Task<string> CallAsync(string systemPrompt, string userMessage, string model)
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

        var completion = await client.CompleteChatAsync(messages, options);
        var content = completion.Value.Content[0].Text.Trim();

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
