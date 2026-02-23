namespace StudyQuest.API.Configuration;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = "StudyQuest";
    public string Audience { get; set; } = "StudyQuestApp";
    public int AccessTokenExpirationMinutes { get; set; } = 60;
    public int RefreshTokenExpirationDays { get; set; } = 30;
}

public class TwilioSettings
{
    public string AccountSid { get; set; } = string.Empty;
    public string AuthToken { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

public class OpenAISettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = "gpt-4o-mini";
    public string ExplanationModel { get; set; } = "gpt-4o";
    public int MaxDailyRequests { get; set; } = 50;
}

public class FirebaseSettings
{
    public string CredentialsPath { get; set; } = string.Empty;
}
