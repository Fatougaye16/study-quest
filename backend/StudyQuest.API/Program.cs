using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using StudyQuest.API.Configuration;
using StudyQuest.API.Data;
using StudyQuest.API.Features.AI;
using StudyQuest.API.Features.AI.Common;
using StudyQuest.API.Features.Auth;
using StudyQuest.API.Features.Auth.Common;
using StudyQuest.API.Features.Enrollments;
using StudyQuest.API.Features.Profile;
using StudyQuest.API.Features.Progress;
using StudyQuest.API.Features.Reminders;
using StudyQuest.API.Features.StudyPlans;
using StudyQuest.API.Features.StudySessions;
using StudyQuest.API.Features.Subjects;
using StudyQuest.API.Features.Timetable;
using StudyQuest.API.Middleware;
using StudyQuest.API.Services.Implementations;
using StudyQuest.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// ── Configuration Binding ──────────────────────────────────────────────────
builder.Services.Configure<JwtSettings>(config.GetSection("JwtSettings"));
builder.Services.Configure<TwilioSettings>(config.GetSection("TwilioSettings"));
builder.Services.Configure<OpenAISettings>(config.GetSection("OpenAISettings"));
builder.Services.Configure<FirebaseSettings>(config.GetSection("FirebaseSettings"));

// ── Database ───────────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

// ── Caching ────────────────────────────────────────────────────────────────
builder.Services.AddMemoryCache();

// ── Authentication (JWT) ───────────────────────────────────────────────────
var jwtSecret = config["JwtSettings:Secret"]
    ?? throw new InvalidOperationException("JwtSettings:Secret is not configured.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["JwtSettings:Issuer"],
        ValidAudience = config["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// ── Services (DI) ──────────────────────────────────────────────────────────
builder.Services.AddScoped<IProgressService, ProgressService>();
builder.Services.AddScoped<IReminderService, ReminderService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<AuthTokenService>();
builder.Services.AddSingleton<OpenAIClient>();

// ── Background Services ────────────────────────────────────────────────────
builder.Services.AddHostedService<ReminderBackgroundService>();

// ── Vertical Slice Infrastructure ─────────────────────────────────────────
builder.Services.AddMediatR(typeof(Program).Assembly);

// ── Controllers + JSON ─────────────────────────────────────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// ── OpenAPI (built-in .NET 10) ──────────────────────────────────────────────
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Info.Title = "Study Quest API";
        document.Info.Version = "v1";
        document.Info.Description = "API for the Study Quest mobile learning app";

        document.Components ??= new();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme()
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Enter your JWT token (without 'Bearer ' prefix)"
        };

        return Task.CompletedTask;
    });
});
builder.Services.AddEndpointsApiExplorer();

// ── CORS ───────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ═══════════════════════════════════════════════════════════════════════════
var app = builder.Build();
// ═══════════════════════════════════════════════════════════════════════════

// ── Middleware Pipeline ────────────────────────────────────────────────────
app.UseGlobalExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Study Quest API");
        options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapAuthEndpoints();
app.MapProfileEndpoints();
app.MapSubjectEndpoints();
app.MapEnrollmentEndpoints();
app.MapTimetableEndpoints();
app.MapStudyPlanEndpoints();
app.MapStudySessionEndpoints();
app.MapProgressEndpoints();
app.MapAIEndpoints();
app.MapReminderEndpoints();

// ── Auto-Migrate & Seed (Development) ──────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.Run();
