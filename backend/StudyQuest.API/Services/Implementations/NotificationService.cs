using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StudyQuest.API.Configuration;
using StudyQuest.API.Data;
using StudyQuest.API.Models;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly AppDbContext _db;
    private readonly FirebaseSettings _settings;
    private readonly ILogger<NotificationService> _logger;
    private readonly IWebHostEnvironment _env;

    public NotificationService(
        AppDbContext db,
        IOptions<FirebaseSettings> settings,
        ILogger<NotificationService> logger,
        IWebHostEnvironment env)
    {
        _db = db;
        _settings = settings.Value;
        _logger = logger;
        _env = env;
    }

    public async Task SendPushNotificationAsync(Guid studentId, string title, string body)
    {
        var tokens = await _db.DeviceTokens
            .AsNoTracking()
            .Where(d => d.StudentId == studentId)
            .Select(d => d.Token)
            .Distinct()
            .ToListAsync();

        if (tokens.Count == 0)
        {
            _logger.LogInformation("No device tokens found for student {StudentId}", studentId);
            return;
        }

        // In development, just log
        if (_env.IsDevelopment() && FirebaseApp.DefaultInstance == null)
        {
            _logger.LogInformation("DEV Push Notification to {StudentId}: {Title} - {Body}", studentId, title, body);
            return;
        }

        try
        {
            var message = new MulticastMessage
            {
                Tokens = tokens,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = new Dictionary<string, string>
                {
                    ["click_action"] = "FLUTTER_NOTIFICATION_CLICK",
                    ["type"] = "study_reminder"
                }
            };

            var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);

            if (response.FailureCount > 0)
            {
                // Remove invalid tokens
                for (int i = 0; i < response.Responses.Count; i++)
                {
                    if (!response.Responses[i].IsSuccess)
                    {
                        await RemoveDeviceTokenAsync(studentId, tokens[i]);
                    }
                }
            }

            _logger.LogInformation(
                "Push notification sent: {Success}/{Total} succeeded",
                response.SuccessCount, tokens.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send push notification to student {StudentId}", studentId);
        }
    }

    public async Task SendPushNotificationToAllAsync(string title, string body)
    {
        var studentIds = await _db.DeviceTokens
            .Select(d => d.StudentId)
            .Distinct()
            .ToListAsync();

        foreach (var studentId in studentIds)
        {
            await SendPushNotificationAsync(studentId, title, body);
        }
    }

    public async Task RegisterDeviceTokenAsync(Guid studentId, string token, string platform)
    {
        if (string.IsNullOrWhiteSpace(token) || token.Length > 512)
            throw new ArgumentException("Invalid device token.", nameof(token));

        if (string.IsNullOrWhiteSpace(platform) || platform.Length > 32)
            throw new ArgumentException("Invalid platform value.", nameof(platform));

        var existing = await _db.DeviceTokens
            .FirstOrDefaultAsync(d => d.Token == token);

        if (existing != null)
        {
            // Update ownership if token already registered
            existing.StudentId = studentId;
            existing.Platform = platform;
        }
        else
        {
            _db.DeviceTokens.Add(new DeviceToken
            {
                Id = Guid.NewGuid(),
                StudentId = studentId,
                Token = token,
                Platform = platform
            });
        }

        await _db.SaveChangesAsync();
    }

    public async Task RemoveDeviceTokenAsync(Guid studentId, string token)
    {
        var deviceToken = await _db.DeviceTokens
            .FirstOrDefaultAsync(d => d.StudentId == studentId && d.Token == token);

        if (deviceToken != null)
        {
            _db.DeviceTokens.Remove(deviceToken);
            await _db.SaveChangesAsync();
        }
    }
}
