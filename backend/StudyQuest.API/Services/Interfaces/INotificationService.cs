namespace StudyQuest.API.Services.Interfaces;

public interface INotificationService
{
    Task SendPushNotificationAsync(Guid studentId, string title, string body);
    Task SendPushNotificationToAllAsync(string title, string body);
    Task RegisterDeviceTokenAsync(Guid studentId, string token, string platform);
    Task RemoveDeviceTokenAsync(Guid studentId, string token);
}
