using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudyQuest.API.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Grade = table.Column<int>(type: "integer", nullable: false),
                    DailyGoalMinutes = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Grade = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Achievements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: false),
                    XPReward = table.Column<int>(type: "integer", nullable: false),
                    UnlockedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Achievements_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Platform = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceTokens_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reminders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IsRecurring = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reminders_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    EnrolledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollments_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentProgress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    XP = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Streak = table.Column<int>(type: "integer", nullable: false),
                    TotalStudyMinutes = table.Column<int>(type: "integer", nullable: false),
                    LastStudyDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentProgress_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentProgress_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudyPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsAIGenerated = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyPlans_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudyPlans_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimetableEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimetableEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimetableEntries_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimetableEntries_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topics_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flashcards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Front = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Back = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    IsAIGenerated = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flashcards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flashcards_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Flashcards_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    IsAIGenerated = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionText = table.Column<string>(type: "text", nullable: false),
                    AnswerText = table.Column<string>(type: "text", nullable: false),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    IsAIGenerated = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    TotalQuestions = table.Column<int>(type: "integer", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quizzes_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Quizzes_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudyPlanItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudyPlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    TopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPlanItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyPlanItems_StudyPlans_StudyPlanId",
                        column: x => x.StudyPlanId,
                        principalTable: "StudyPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudyPlanItems_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudySessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    TopicId = table.Column<Guid>(type: "uuid", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudySessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudySessions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudySessions_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudySessions_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "QuizQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuizId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionText = table.Column<string>(type: "text", nullable: false),
                    OptionsJson = table.Column<string>(type: "text", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "text", nullable: false),
                    StudentAnswer = table.Column<string>(type: "text", nullable: true),
                    Explanation = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizQuestions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "Color", "Description", "Grade", "Name" },
                values: new object[,]
                {
                    { new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209"), "#ec4899", "Biology for Grade 12", 12, "Biology" },
                    { new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23"), "#22c55e", "Science for Grade 10", 10, "Science" },
                    { new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495"), "#f59e0b", "Chemistry for Grade 12", 12, "Chemistry" },
                    { new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d"), "#f59e0b", "Chemistry for Grade 10", 10, "Chemistry" },
                    { new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa"), "#ef4444", "Mathematics for Grade 11", 11, "Mathematics" },
                    { new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e"), "#3b82f6", "English for Grade 12", 12, "English" },
                    { new Guid("629825b8-c2d2-7aee-d163-0be88495e272"), "#06b6d4", "Geography for Grade 11", 11, "Geography" },
                    { new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e"), "#a855f7", "Physics for Grade 12", 12, "Physics" },
                    { new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe"), "#84cc16", "Agriculture for Grade 12", 12, "Agriculture" },
                    { new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e"), "#ec4899", "Biology for Grade 10", 10, "Biology" },
                    { new Guid("76324b9e-5fc2-7160-7522-932866c0a77a"), "#84cc16", "Agriculture for Grade 11", 11, "Agriculture" },
                    { new Guid("7a475323-7df5-f613-6449-fcfad4461d7c"), "#06b6d4", "Geography for Grade 12", 12, "Geography" },
                    { new Guid("82786b28-5a6d-b0b6-194d-ab131764064d"), "#22c55e", "Science for Grade 11", 11, "Science" },
                    { new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8"), "#22c55e", "Science for Grade 12", 12, "Science" },
                    { new Guid("8c039749-e2dc-12fc-0757-80e63a219052"), "#a855f7", "Physics for Grade 11", 11, "Physics" },
                    { new Guid("99b68af6-34fc-713c-884f-c018c6b17d72"), "#3b82f6", "English for Grade 11", 11, "English" },
                    { new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959"), "#ec4899", "Biology for Grade 11", 11, "Biology" },
                    { new Guid("caea75ab-707a-872b-c92f-5500afab7afb"), "#a855f7", "Physics for Grade 10", 10, "Physics" },
                    { new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0"), "#84cc16", "Agriculture for Grade 10", 10, "Agriculture" },
                    { new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba"), "#ef4444", "Mathematics for Grade 12", 12, "Mathematics" },
                    { new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4"), "#f59e0b", "Chemistry for Grade 11", 11, "Chemistry" },
                    { new Guid("f8380400-ac39-9946-15e0-894ba7e520e6"), "#06b6d4", "Geography for Grade 10", 10, "Geography" },
                    { new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8"), "#ef4444", "Mathematics for Grade 10", 10, "Mathematics" },
                    { new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf"), "#3b82f6", "English for Grade 10", 10, "English" }
                });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "Description", "Name", "Order", "SubjectId" },
                values: new object[,]
                {
                    { new Guid("01b83c0a-da1b-d3ee-63c6-a7223a562a70"), "Optical Phenomena — Science Grade 12", "Optical Phenomena", 7, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") },
                    { new Guid("01c89125-6054-4ff5-732e-e588e2723bd7"), "Ideal Gases — Science Grade 11", "Ideal Gases", 7, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("0418668d-5ea4-5b7c-9fa6-1f46a2b4ae82"), "Summary Writing — English Grade 10", "Summary Writing", 2, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("050fd061-3d93-5f39-8a4a-c324dfe3ae55"), "Geomorphology (Surface Processes) — Geography Grade 10", "Geomorphology (Surface Processes)", 2, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("0542c6bb-6c8c-3d9a-e9fc-af56459051ed"), "Quantitative Aspects of Chemical Change — Science Grade 11", "Quantitative Aspects of Chemical Change", 8, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("0618c3f0-291c-6ede-9cd8-2cb9f28e7d8c"), "Economic Geography (Secondary, Tertiary) — Geography Grade 11", "Economic Geography (Secondary, Tertiary)", 5, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("0651b6ce-8c9c-a631-9a5c-832a62d3f4ce"), "Vectors and Scalars — Science Grade 11", "Vectors and Scalars", 1, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("0780c4e7-f729-4e93-dcd4-e97c01c6abe6"), "Animal Production (Breeding, Genetics) — Agriculture Grade 12", "Animal Production (Breeding, Genetics)", 3, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("09e08e66-3440-ad62-4e19-33609a335577"), "Literature: Poetry Analysis — English Grade 11", "Literature: Poetry Analysis", 4, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("0a968297-5849-7e33-62a7-42614e560e53"), "Literature: Drama — English Grade 10", "Literature: Drama", 6, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("0b886e2a-37b2-806a-17ff-af7cbfd75b1f"), "Plant Studies (Structure, Growth) — Agriculture Grade 10", "Plant Studies (Structure, Growth)", 3, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("0b9021c9-d560-a851-92cc-d490c449b774"), "Resources and Sustainability (Energy) — Geography Grade 12", "Resources and Sustainability (Energy)", 6, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("0d051784-8b5c-c90f-7db0-eea0b82325e6"), "Biosphere to Ecosystems (Population Ecology) — Biology Grade 11", "Biosphere to Ecosystems (Population Ecology)", 6, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("0e351883-931d-6518-94a2-8bfa033b954c"), "Momentum and Impulse — Science Grade 12", "Momentum and Impulse", 1, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") },
                    { new Guid("0f004646-1571-b5d5-f8fe-ae1d2a422952"), "Excretion in Humans — Biology Grade 10", "Excretion in Humans", 10, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("12c332eb-1189-88ff-d2cf-1491009c5e64"), "Electric Circuits (Advanced) — Physics Grade 12", "Electric Circuits (Advanced)", 6, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("14ce031f-6c0a-6af2-279f-4b6e66c27169"), "Transactional Writing (Speech, Interview) — English Grade 12", "Transactional Writing (Speech, Interview)", 7, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("197831c0-c31f-305f-5fca-6809f0d96f7f"), "Work, Energy and Power — Physics Grade 11", "Work, Energy and Power", 4, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("214ed8ad-1898-327b-07df-f8ae143bbc41"), "Exam Preparation and Map Skills — Geography Grade 12", "Exam Preparation and Map Skills", 10, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("231ced44-3f9d-42fa-aa02-7ace491139be"), "Statistics — Mathematics Grade 10", "Statistics", 9, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("26436f95-e13e-e1db-1a17-740a78be7ce5"), "Finance and Growth — Mathematics Grade 10", "Finance and Growth", 5, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("2887d4df-b719-9850-3b71-d91448d76cd0"), "Electrodynamics (Motors, Generators) — Physics Grade 12", "Electrodynamics (Motors, Generators)", 7, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("288d8ea0-c1fb-b3e6-7972-dd554017287a"), "Plant Production (Advanced Techniques) — Agriculture Grade 12", "Plant Production (Advanced Techniques)", 2, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("297b9215-1ec3-f632-2c1a-7e790fcdfb57"), "Mechanical Energy — Science Grade 11", "Mechanical Energy", 3, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("2a981624-ea72-0c90-5f56-cebe5168cb33"), "Tourism and the Environment — Geography Grade 11", "Tourism and the Environment", 10, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("2d679eb7-979c-625c-f1d5-2a46d2eaba03"), "Magnetism and Electrostatics — Chemistry Grade 10", "Magnetism and Electrostatics", 6, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("2d9f2e39-601b-8d59-3057-19d92171c6cf"), "Electrochemistry (Galvanic, Electrolytic Cells) — Chemistry Grade 12", "Electrochemistry (Galvanic, Electrolytic Cells)", 7, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("2e1204bc-b7c0-831f-bf12-d8c8dbd271e5"), "Euclidean Geometry (Proportionality, Similarity) — Mathematics Grade 12", "Euclidean Geometry (Proportionality, Similarity)", 7, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("2ec7ecd2-b943-bd77-8efc-b01725d6133a"), "Meiosis and Genetics — Biology Grade 12", "Meiosis and Genetics", 2, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("2f03fddb-8947-45ea-224c-cd9e9cc8514e"), "Chemical Equilibrium (Introduction) — Chemistry Grade 11", "Chemical Equilibrium (Introduction)", 9, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("2f5c42c7-5b66-043f-a338-ff1489cb84b4"), "Transactional Writing (Formal Letters, Reviews) — English Grade 11", "Transactional Writing (Formal Letters, Reviews)", 8, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("2ff2ff99-92d5-5e48-22cd-999d6bcdbe81"), "Literature: Novel Study — English Grade 11", "Literature: Novel Study", 3, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("32543d9f-dba6-d777-98b6-fb1248a26f82"), "Excretion in Humans (Urinary System) — Biology Grade 11", "Excretion in Humans (Urinary System)", 8, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("3284ed1c-bfd2-e1b4-39ef-b396f35857ad"), "Comprehension and Language — English Grade 11", "Comprehension and Language", 1, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("333d6e57-1224-bdd4-f8ed-972af8e887dd"), "The Atmosphere and Weather — Geography Grade 10", "The Atmosphere and Weather", 1, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("34986eb4-6e8f-4ae3-c52b-8c4dd81799e3"), "Electrostatics (Electric Fields) — Physics Grade 12", "Electrostatics (Electric Fields)", 5, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("34ac8896-f538-98f6-045b-d6eac23845fc"), "Vertical Projectile Motion — Physics Grade 12", "Vertical Projectile Motion", 2, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("35c791a5-93f6-d9c6-fba4-2813a98e1c27"), "Doppler Effect — Science Grade 12", "Doppler Effect", 5, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") },
                    { new Guid("37cc5473-c3e9-fdb3-8066-e41147b900d3"), "Human Impact on the Environment — Biology Grade 12", "Human Impact on the Environment", 10, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("394a31f0-6f35-1637-f69a-8cb7b9a306b5"), "Animal Studies (Classification, Nutrition) — Agriculture Grade 10", "Animal Studies (Classification, Nutrition)", 4, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("3b2fc17f-e7dc-f10e-6aef-d74f8f70428e"), "Water Resources — Geography Grade 10", "Water Resources", 5, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("3cd5f9e7-e4d7-93ad-6b57-8960015fda54"), "Scientific Method and Skills — Science Grade 10", "Scientific Method and Skills", 1, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("3df6b272-c703-ea94-590d-39cf4c886223"), "Acids and Bases (pH, Titrations) — Chemistry Grade 12", "Acids and Bases (pH, Titrations)", 6, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("3e7dd8ba-27be-47e9-7819-37c77520a90d"), "Exam Preparation and Techniques — English Grade 12", "Exam Preparation and Techniques", 10, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("418e8e0c-5c2c-7bb2-6827-b00bbf80bb39"), "Map Skills and Techniques — Geography Grade 10", "Map Skills and Techniques", 3, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("41c2f514-4908-cdb9-8cf8-ab12c6dd82bb"), "Atomic Combinations (Molecular Structure) — Chemistry Grade 11", "Atomic Combinations (Molecular Structure)", 1, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("421ebeb7-f096-909d-bbed-88241ea8f5fd"), "Electric Circuits (Internal Resistance) — Physics Grade 11", "Electric Circuits (Internal Resistance)", 9, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("45a27f2a-c0fe-2439-003e-bbb6b6a6fcf5"), "Environmental Impact of Agriculture — Agriculture Grade 11", "Environmental Impact of Agriculture", 9, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("46016da9-d7e5-79c3-42ed-7f9e781ad409"), "Gravity and Mechanical Energy — Physics Grade 10", "Gravity and Mechanical Energy", 4, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("469a9107-71ed-3a47-6f04-4c66303ac5e3"), "Development Geography — Geography Grade 11", "Development Geography", 6, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("46a1de31-acbc-fa47-f1d9-a4c405ad4c06"), "Electrodynamics — Science Grade 12", "Electrodynamics", 6, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") },
                    { new Guid("48751395-d242-5f84-48c6-dd188eb243e3"), "Rate of Reactions (Factors, Mechanism) — Chemistry Grade 12", "Rate of Reactions (Factors, Mechanism)", 4, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("49274ac5-49e2-98a0-0f81-21ed00496707"), "Comprehension and Language — English Grade 12", "Comprehension and Language", 1, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("4a7b80cb-3322-d226-5d04-7bef5cd1427d"), "Reproduction in Vertebrates — Biology Grade 11", "Reproduction in Vertebrates", 10, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("4c9b3c0e-ea03-6dc1-edca-b11ae4d2d37d"), "Electromagnetic Radiation — Science Grade 10", "Electromagnetic Radiation", 7, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("504dfee0-22b2-3f84-66f8-0866506b2167"), "Physical and Chemical Change — Chemistry Grade 10", "Physical and Chemical Change", 4, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("50de216c-0ddc-ae74-f8ad-5444e814b235"), "Biodiversity and Classification (Detailed) — Biology Grade 11", "Biodiversity and Classification (Detailed)", 1, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("51359da3-e266-2b11-205d-1d3e634654c7"), "Population Studies — Geography Grade 10", "Population Studies", 4, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("51ef4d2e-6f88-4f2f-1fd2-185a6afe78b9"), "Equations and Inequalities — Mathematics Grade 11", "Equations and Inequalities", 2, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("5262754b-d02f-be07-3cc5-81f92daf5e45"), "Animal Reproduction — Agriculture Grade 12", "Animal Reproduction", 4, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("52a5fcb0-3134-3b41-f1ae-d181ba532f09"), "Analytical Geometry — Mathematics Grade 10", "Analytical Geometry", 7, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("532239c5-336a-48f8-cf92-a9651fc66796"), "Light (Refraction, Snell's Law) — Physics Grade 11", "Light (Refraction, Snell's Law)", 7, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("53da0239-03de-447c-e7d5-0ac404ee6ce8"), "Gaseous Exchange — Biology Grade 10", "Gaseous Exchange", 9, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("54110ec1-7c50-6e4c-b811-c4dd1df5485a"), "Electric Circuits — Physics Grade 10", "Electric Circuits", 9, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("54cebaf0-c5b2-446b-017d-a3b10deb3679"), "Visual Literacy (Cartoons, Advertisements) — English Grade 12", "Visual Literacy (Cartoons, Advertisements)", 8, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("58648486-b170-4c7d-f262-5ae496ea71de"), "Endocrine System — Biology Grade 12", "Endocrine System", 6, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("58ecad01-ddfb-38eb-e236-388b371ea195"), "Statistics (Counting Principles) — Mathematics Grade 12", "Statistics (Counting Principles)", 8, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("59d26f10-1011-730a-bb2a-da01ad019205"), "Rate of Reactions — Chemistry Grade 11", "Rate of Reactions", 8, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("5a6e7f96-d196-5905-45a2-408144c11403"), "Algebraic Expressions — Mathematics Grade 10", "Algebraic Expressions", 1, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("5a75752a-5a4c-ab5e-cc8f-b5ec4d3cea1b"), "Optical Phenomena (Photoelectric Effect) — Physics Grade 12", "Optical Phenomena (Photoelectric Effect)", 8, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("5a765634-6417-4f5e-1a33-b45063aa04f3"), "Organic Chemistry (Polymers, Plastics) — Chemistry Grade 12", "Organic Chemistry (Polymers, Plastics)", 3, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("5ea08431-8e62-9a42-e6d8-2be18c18db18"), "Sound (Doppler Effect intro) — Physics Grade 11", "Sound (Doppler Effect intro)", 6, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("5fbc9b9f-3366-4659-dd89-9cf00a28b73a"), "Quantitative Aspects (Mole Concept) — Chemistry Grade 10", "Quantitative Aspects (Mole Concept)", 9, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("60f06aab-ede4-cd69-1197-dfe54ecd27ec"), "Nervous System and Senses — Biology Grade 12", "Nervous System and Senses", 5, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("61542b76-fc35-064b-7dfb-ed661ab26961"), "Homeostasis — Biology Grade 12", "Homeostasis", 7, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("615c5357-e20c-8d6a-db74-dc5e43b353ad"), "Soil Science (Formation, Types) — Agriculture Grade 10", "Soil Science (Formation, Types)", 2, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("6288f6fc-0672-c122-b2b9-df5d967376bc"), "Plant Production (Crop Management) — Agriculture Grade 11", "Plant Production (Crop Management)", 2, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("639bf567-8933-910e-f99e-3033e568d205"), "Intermolecular Forces — Science Grade 11", "Intermolecular Forces", 6, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("63ad600b-8bca-1b26-eade-64d0a219a279"), "Functions and Graphs — Mathematics Grade 10", "Functions and Graphs", 4, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("6c6e3005-6ee3-bbe6-d150-eb76b67a30ca"), "Electrochemistry — Science Grade 12", "Electrochemistry", 8, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") },
                    { new Guid("6d43e975-bfc9-fca1-c243-de13a1063642"), "Units and Measurements — Physics Grade 10", "Units and Measurements", 1, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("6dfcc605-241e-9b04-b6bb-35aed8cc13cc"), "Magnetism — Physics Grade 10", "Magnetism", 10, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("6ef9764f-16b5-a8de-e373-6f9f7bbf35e4"), "Cell Division (Meiosis) — Biology Grade 11", "Cell Division (Meiosis)", 9, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("6f52470d-b0fa-604c-ad84-15bae8e1a8b2"), "Quantitative Aspects (Stoichiometry) — Chemistry Grade 11", "Quantitative Aspects (Stoichiometry)", 4, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("70d9d2a8-636e-3488-4edb-cc64cf300f77"), "Finance, Growth and Decay (Annuities) — Mathematics Grade 12", "Finance, Growth and Decay (Annuities)", 3, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("70f888ef-b3d2-f258-7cbe-3feab9adba0d"), "Solutions and Solubility — Chemistry Grade 10", "Solutions and Solubility", 8, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("716f6983-9029-6713-64bd-776749d4500c"), "Momentum — Physics Grade 11", "Momentum", 3, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("718dcda4-4d62-267f-8ac8-f05a1c9d2726"), "Human Evolution — Biology Grade 12", "Human Evolution", 4, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("72272db0-2456-e9d7-4e47-7eab7c5a961a"), "Equations and Inequalities — Mathematics Grade 10", "Equations and Inequalities", 2, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("72499899-2012-50f0-baca-b8267f238437"), "Statistics (Regression, Correlation) — Mathematics Grade 11", "Statistics (Regression, Correlation)", 9, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("7431c6fe-376b-4b07-b480-d36203e1e9df"), "Energy and Chemical Change — Chemistry Grade 10", "Energy and Chemical Change", 10, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("74614f4b-ca77-9771-2023-a94dbfd4da03"), "Cell Division (Mitosis) — Biology Grade 10", "Cell Division (Mitosis)", 3, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("7482d60f-e6a9-4de5-e0c0-9a664877752f"), "Settlement Geography — Geography Grade 10", "Settlement Geography", 7, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("74ebf86b-b31b-5327-dc77-d43bc9f9ee18"), "Chemical Equilibrium (Le Chatelier's) — Chemistry Grade 12", "Chemical Equilibrium (Le Chatelier's)", 5, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("7571f553-46da-20c5-75db-9943be088f12"), "Literature: Poetry (Exam Prep) — English Grade 12", "Literature: Poetry (Exam Prep)", 4, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("76040032-7e9e-5cc1-a3a8-41225f37ed3b"), "Trigonometry (Compound and Double Angles) — Mathematics Grade 12", "Trigonometry (Compound and Double Angles)", 5, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("76f4b310-7843-c068-1452-a3c0c8352ade"), "Agricultural Economics (Business Plans) — Agriculture Grade 12", "Agricultural Economics (Business Plans)", 5, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("771514d7-f986-ab4f-9253-413fb16300f1"), "Energy and Chemical Change (Enthalpy) — Chemistry Grade 11", "Energy and Chemical Change (Enthalpy)", 5, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("781aede2-451d-e8a1-0583-bdb2657a62b4"), "Organic Chemistry (Nomenclature) — Chemistry Grade 12", "Organic Chemistry (Nomenclature)", 1, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("792e1ddb-b494-4edd-dbfe-0e51a35071d5"), "Biosphere to Ecosystems — Biology Grade 10", "Biosphere to Ecosystems", 7, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("7a6038e9-d2e5-cbc0-fe36-4d39c79939a5"), "Forces and Newton's Laws — Physics Grade 10", "Forces and Newton's Laws", 3, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("7d8d7a19-7d81-d3c9-47c7-ac7cebadf41a"), "Euclidean Geometry (Circle Theorems) — Mathematics Grade 11", "Euclidean Geometry (Circle Theorems)", 8, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("7dfd8bfa-4134-5dc1-7041-04661e427d6b"), "Chemical Bonding — Chemistry Grade 10", "Chemical Bonding", 3, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("7f275686-7a4e-77cf-fb60-f5e7020f1a06"), "Climate and Weather (South Africa) — Geography Grade 10", "Climate and Weather (South Africa)", 6, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("7f909e11-85e1-88f1-2975-2522edaa2bf1"), "Animal Nutrition and Feeding — Agriculture Grade 11", "Animal Nutrition and Feeding", 4, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("81004de3-ba78-1bbd-8090-be0cda89866c"), "Representing Chemical Change — Chemistry Grade 10", "Representing Chemical Change", 5, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("81788eec-1a1c-14e9-548c-44f7e31e74c4"), "Water Management (Irrigation) — Agriculture Grade 10", "Water Management (Irrigation)", 7, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("82b597dd-e1dc-3398-27ac-c4a9bbe80b44"), "Hazards and Disasters (Risk Management) — Geography Grade 12", "Hazards and Disasters (Risk Management)", 7, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("832be98d-ff03-a21e-b1ec-34e30faca608"), "The Atom — Science Grade 10", "The Atom", 4, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("83321fec-3af1-8082-dbbf-9aa7cf4acebb"), "Resources and Sustainability — Geography Grade 11", "Resources and Sustainability", 7, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("837f11aa-9d94-bd5c-6db6-fb2e8ec2c506"), "Electric Circuits (Series and Parallel) — Science Grade 11", "Electric Circuits (Series and Parallel)", 10, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("839e9bb7-cb14-f554-d1c5-cd0dde7f5784"), "Population (Migration, Urbanization) — Geography Grade 11", "Population (Migration, Urbanization)", 4, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("854a8a9e-7b89-c9fb-4811-4c5d9d19023b"), "Gaseous Exchange (Detailed) — Biology Grade 11", "Gaseous Exchange (Detailed)", 7, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("867d475d-0238-17ca-510c-320f7f616248"), "Soil Science (Advanced, Land Use) — Agriculture Grade 12", "Soil Science (Advanced, Land Use)", 1, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("86ce8d84-408d-9d4c-adf2-8d16d69423e4"), "Types of Reactions (Acid-Base, Redox) — Chemistry Grade 11", "Types of Reactions (Acid-Base, Redox)", 6, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("88d3aed4-7aa6-aeaf-be74-3f3c44f8fe91"), "Response of the Immune System — Biology Grade 12", "Response of the Immune System", 9, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("8a4cb3cf-d79d-8b4e-e9cc-bd45a903fb90"), "Cell Structure and Function — Biology Grade 10", "Cell Structure and Function", 2, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("8b2a8163-39ae-a6f0-d25d-5c05a22bd365"), "Exam Revision and Problem Solving — Chemistry Grade 12", "Exam Revision and Problem Solving", 10, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("8b929c5a-3418-4a3f-982c-47939f2de55b"), "Periodic Table — Chemistry Grade 10", "Periodic Table", 2, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("8d715e79-be6e-5e13-10c9-2185d6fa5076"), "Plant and Animal Tissues (Advanced) — Biology Grade 11", "Plant and Animal Tissues (Advanced)", 3, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("8d739583-8bb6-1144-d638-3130c8670a85"), "Doppler Effect — Physics Grade 12", "Doppler Effect", 4, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("8df031a3-dc4a-74cb-8cea-84482e97e0cc"), "Farm Planning and Layout — Agriculture Grade 10", "Farm Planning and Layout", 6, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("8fe6402d-ab28-170b-b190-59519649e6a0"), "Measurement — Mathematics Grade 10", "Measurement", 11, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("900b0581-a884-0700-5c2f-1ee846b2d9a6"), "Environmental Geography — Geography Grade 10", "Environmental Geography", 9, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("91a31b80-6aaa-df03-65d7-616e68adcbb9"), "Newton's Laws of Motion — Science Grade 11", "Newton's Laws of Motion", 2, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("91d90182-e3de-3d69-34d2-fd07abb6710a"), "Farm Management (Labour, Finance) — Agriculture Grade 12", "Farm Management (Labour, Finance)", 6, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("92141092-e9bc-e505-2aca-35d46b603441"), "Sequences and Series — Mathematics Grade 12", "Sequences and Series", 1, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("921d990b-d5de-edf1-c136-335b1b60a23d"), "Geomorphology (Fluvial, Coastal Landscapes) — Geography Grade 12", "Geomorphology (Fluvial, Coastal Landscapes)", 2, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("92579598-4144-1b60-efe1-d108270bee32"), "Spatial Planning — Geography Grade 12", "Spatial Planning", 9, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("92bfd75c-ad93-0d18-c3bf-1dbfc0062da0"), "Electric Circuits — Science Grade 10", "Electric Circuits", 10, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("92c8e20b-a633-3a98-a238-b618bc716f8b"), "Acids and Bases — Science Grade 12", "Acids and Bases", 10, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") },
                    { new Guid("9353cb3e-0beb-f23b-c2f4-214538abf8be"), "Creative Writing (Narrative, Descriptive) — English Grade 10", "Creative Writing (Narrative, Descriptive)", 7, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("93891937-80ee-7db9-5ded-c76f297baf24"), "Introduction to Agriculture — Agriculture Grade 10", "Introduction to Agriculture", 1, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("93dd70db-f471-0287-6b4a-088b0afdc4e9"), "Work, Energy and Power (Advanced) — Physics Grade 12", "Work, Energy and Power (Advanced)", 3, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("94a0ed13-a568-1e7f-94d6-a362cbb000a2"), "Finance, Growth and Decay — Mathematics Grade 11", "Finance, Growth and Decay", 5, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("958ef24a-1848-11fc-7260-6ca5fb0e08b5"), "Map Work and Calculations — Geography Grade 11", "Map Work and Calculations", 3, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("972dc873-9109-24de-9ff9-fd076b1783d4"), "Momentum and Impulse (Advanced) — Physics Grade 12", "Momentum and Impulse (Advanced)", 1, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("982a1d25-91f5-061f-eec2-903b4fd07a5b"), "Nuclear Physics — Physics Grade 12", "Nuclear Physics", 10, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("988c83c1-55da-d3e2-12aa-de74d132c448"), "Vertical Projectile Motion — Science Grade 12", "Vertical Projectile Motion", 2, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") },
                    { new Guid("98dd9392-0a4f-01ca-5bf8-3070c618379b"), "Quantitative Analysis — Chemistry Grade 12", "Quantitative Analysis", 9, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("9a7e23fa-7cd8-7df9-3ebf-04d780ad7bb8"), "GIS and Remote Sensing (Introduction) — Geography Grade 10", "GIS and Remote Sensing (Introduction)", 10, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("9ac07bb9-773c-3e1a-3dbf-fd306f613c94"), "Climate and Weather (Global Patterns) — Geography Grade 11", "Climate and Weather (Global Patterns)", 1, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("9b431cb9-474e-8611-1199-e207dd49740a"), "Probability (Tree Diagrams, Contingency Tables) — Mathematics Grade 11", "Probability (Tree Diagrams, Contingency Tables)", 10, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("9c623dd8-3a7c-8aad-0601-f0a84074c3e9"), "Electromagnetic Radiation (Atomic Spectra) — Physics Grade 12", "Electromagnetic Radiation (Atomic Spectra)", 9, new Guid("65ac38a6-5a9f-22e8-0de0-2181565e0d5e") },
                    { new Guid("9d5c3fe7-8b62-b3c7-dde8-82fb9da0350b"), "Analytical Geometry (Circles, Tangents) — Mathematics Grade 12", "Analytical Geometry (Circles, Tangents)", 6, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("9d6e7e68-fc31-da6c-7467-2adbc7ee1638"), "Grammar, Editing and Language Structures — English Grade 11", "Grammar, Editing and Language Structures", 10, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("9d81c63d-ced0-fb8a-0dec-22f293b69ae4"), "Visual Literacy and Advertising — English Grade 11", "Visual Literacy and Advertising", 9, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("9df9e604-aa11-b7f5-eefa-56adc54d807c"), "Creative Writing (Argumentative, Reflective) — English Grade 12", "Creative Writing (Argumentative, Reflective)", 6, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("9f3ff1df-ad72-2908-3f6b-2bc02348a496"), "Geomorphology (Drainage Systems) — Geography Grade 11", "Geomorphology (Drainage Systems)", 2, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("9fa00bb7-65bf-0e73-7a34-d9f776af9cdb"), "Animal Production (Livestock Management) — Agriculture Grade 11", "Animal Production (Livestock Management)", 3, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("a10948f2-bb20-75fa-7829-1d374f32ff72"), "Summary Writing — English Grade 11", "Summary Writing", 2, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("a1ec96b8-4bf8-6b02-4a31-694e5157a900"), "Electrostatics (Electric Fields) — Science Grade 11", "Electrostatics (Electric Fields)", 9, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("a229c887-61eb-61c3-82a1-8fc37af2a58e"), "Economic Geography of South Africa — Geography Grade 12", "Economic Geography of South Africa", 4, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("a27e3586-6d0e-ba1e-0584-df1d3557d392"), "Number Patterns and Sequences — Mathematics Grade 11", "Number Patterns and Sequences", 3, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("a3de0b84-1ab3-2f97-5ddb-2a72dffd9fb2"), "Evolution (Natural Selection, Speciation) — Biology Grade 12", "Evolution (Natural Selection, Speciation)", 3, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("a467b2c6-0c89-9bf5-2d65-016d07ea87e3"), "Hazards and Disasters — Geography Grade 11", "Hazards and Disasters", 8, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("a4c9b011-4ccd-76c9-ddbf-7290777f735f"), "Indigenous Knowledge Systems in Agriculture — Agriculture Grade 11", "Indigenous Knowledge Systems in Agriculture", 10, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("a8039e66-9ee4-9b91-0790-b563c3701ded"), "Literature: Drama (Exam Prep) — English Grade 12", "Literature: Drama (Exam Prep)", 5, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("a82eb8a4-4575-69b4-27ba-42261f0dd677"), "Energy Flow and Nutrient Cycling — Biology Grade 10", "Energy Flow and Nutrient Cycling", 8, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("a96f56d1-fcda-bdb6-e26a-add35e069baf"), "Support and Transport Systems in Plants — Biology Grade 11", "Support and Transport Systems in Plants", 4, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("aad284ca-133a-8645-69d8-b8b3d90e1cb9"), "Intermolecular Forces (Advanced) — Chemistry Grade 11", "Intermolecular Forces (Advanced)", 2, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("ab210d31-92e1-b37c-ef4a-aaa6e59542d8"), "Economic Geography (Primary Activities) — Geography Grade 10", "Economic Geography (Primary Activities)", 8, new Guid("f8380400-ac39-9946-15e0-894ba7e520e6") },
                    { new Guid("abaa3fa4-c13c-5e90-6fda-f781cd3c883a"), "Creative Writing (Essays, Narratives) — English Grade 11", "Creative Writing (Essays, Narratives)", 7, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("acd2944d-8102-2c04-b518-e0520dc91d6b"), "Electrostatics — Science Grade 10", "Electrostatics", 9, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("adaef9b7-1a85-d8bc-c690-83288f70c2d6"), "Functions (Parabola, Hyperbola, Exponential) — Mathematics Grade 11", "Functions (Parabola, Hyperbola, Exponential)", 4, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("ae58b202-01db-0196-287e-57f047e49539"), "Differential Calculus — Mathematics Grade 12", "Differential Calculus", 4, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("af4be5ec-c9c7-cb98-28e8-555888526a6a"), "Mechanization and Technology — Agriculture Grade 11", "Mechanization and Technology", 8, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("af6df267-d7f7-40aa-bac4-97a62cdb4812"), "Functions and Inverse Functions — Mathematics Grade 12", "Functions and Inverse Functions", 2, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("b03e4dd7-c5ee-6122-294d-5cf2c6543156"), "Chemical Bonding — Science Grade 10", "Chemical Bonding", 5, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("b0b02cb0-f0eb-a064-ee36-75ea48b10fe5"), "Waves, Sound and Light — Science Grade 11", "Waves, Sound and Light", 4, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("b189674c-4e35-7dae-e043-007edf702650"), "Literature: Novel Study — English Grade 10", "Literature: Novel Study", 3, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("b25ad283-d4a3-a8e3-e2a3-3b2680f3be1e"), "Transverse Waves — Science Grade 10", "Transverse Waves", 6, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("b44f7958-dee5-6736-648c-6e871f906d3f"), "Vectors in Two Dimensions — Physics Grade 11", "Vectors in Two Dimensions", 1, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("b4a4b05f-7229-0b1e-5112-5fbe330f87c5"), "Euclidean Geometry — Mathematics Grade 10", "Euclidean Geometry", 8, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("b578853a-5577-6231-4ead-4700bcdbe6fd"), "Comprehension and Language — English Grade 10", "Comprehension and Language", 1, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("bb7b2c32-22c4-7ee8-84bb-1d80f3312ea3"), "Trigonometry — Mathematics Grade 10", "Trigonometry", 6, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("bbf12c25-de6f-1e64-1c1a-4cd09ccdc5c7"), "Biodiversity and Classification — Biology Grade 10", "Biodiversity and Classification", 5, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("bf10904f-67bc-7ab4-8118-404a265badc2"), "Electromagnetism — Physics Grade 11", "Electromagnetism", 10, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("bff01c9b-9947-3576-9732-8ebe11eabebd"), "DNA, RNA and Protein Synthesis — Biology Grade 12", "DNA, RNA and Protein Synthesis", 1, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("c094121f-5733-f5bf-3495-25546af2bc95"), "Atomic Structure — Chemistry Grade 10", "Atomic Structure", 1, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("c0a479ce-2f71-6717-c032-3daaf8641b3a"), "Intermolecular Forces (Intro) — Chemistry Grade 10", "Intermolecular Forces (Intro)", 7, new Guid("45c2e4c9-1dad-eb06-8108-5a7b253db61d") },
                    { new Guid("c0e20d79-e15b-7eac-ce2c-2c225464fbc7"), "Transverse Pulses and Waves — Physics Grade 10", "Transverse Pulses and Waves", 5, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("c15d5a7b-0450-1976-7333-fb5cb0d0a05c"), "Climate Change and Agriculture — Agriculture Grade 12", "Climate Change and Agriculture", 8, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("c2c2dc37-85b5-b179-a185-3a817633d54a"), "Soil Science (Fertility, Conservation) — Agriculture Grade 11", "Soil Science (Fertility, Conservation)", 1, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("c306c6c6-0139-cef5-3df7-8ccaaf61903c"), "Electrochemistry (Introduction) — Chemistry Grade 11", "Electrochemistry (Introduction)", 10, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("c482fee0-eaa6-b70d-b94e-19495a18b0c8"), "Literature: Short Stories Analysis — English Grade 11", "Literature: Short Stories Analysis", 5, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("c593755e-43a2-bee1-efd8-ab112688b7d3"), "Support and Transport Systems in Animals — Biology Grade 11", "Support and Transport Systems in Animals", 5, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("c5dfcf02-6cc0-0435-efd5-4a5936dbaa16"), "Entrepreneurship in Agriculture — Agriculture Grade 12", "Entrepreneurship in Agriculture", 10, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("c5e93b39-4278-eae7-3ac5-22b60c087068"), "Probability — Mathematics Grade 10", "Probability", 10, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("c637c9c3-e71e-ef19-b93c-038e1060c339"), "Summary Writing — English Grade 12", "Summary Writing", 2, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("c677ac5a-d29a-8fb7-a71d-3986e156e8ab"), "GIS (Advanced Applications) — Geography Grade 12", "GIS (Advanced Applications)", 8, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("c6cf9add-0ec4-5b53-e4f8-65a523a9749b"), "Farm Management and Planning — Agriculture Grade 11", "Farm Management and Planning", 7, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("c8259e20-c95c-2239-fd1f-c9d15bfababc"), "The Chemical Industry (Fertilizers) — Chemistry Grade 12", "The Chemical Industry (Fertilizers)", 8, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("c86aaefa-61f2-1f82-6ecd-9243160261b4"), "Sound Waves — Physics Grade 10", "Sound Waves", 6, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("c8ad3461-3649-93da-ed28-595297e891c3"), "Agricultural Economics (Basic) — Agriculture Grade 10", "Agricultural Economics (Basic)", 5, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("c8b0d99d-86a7-208f-43a9-f94003608d3b"), "Literature: Novel Study (Exam Prep) — English Grade 12", "Literature: Novel Study (Exam Prep)", 3, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("c8cfe728-fe0a-0883-ad14-0057aef8697e"), "Work, Energy and Power — Science Grade 12", "Work, Energy and Power", 4, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") },
                    { new Guid("c8e3da43-f33b-c2f1-d9db-f6553cf6ce64"), "Motion in One Dimension — Physics Grade 10", "Motion in One Dimension", 2, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("c908eff7-1df5-09a9-f5ae-3617899b6f31"), "Probability (Fundamental Counting Principle) — Mathematics Grade 12", "Probability (Fundamental Counting Principle)", 9, new Guid("eb63371f-c483-4b34-cf09-1f1f6825f8ba") },
                    { new Guid("c99c9b78-3b6b-5e2f-d6b3-57cb0918ec66"), "Pest and Disease Control — Agriculture Grade 10", "Pest and Disease Control", 8, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("cabfdf95-7580-c4f0-9d96-e354a9d1d823"), "Waves (Interference, Diffraction) — Physics Grade 11", "Waves (Interference, Diffraction)", 5, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("cc0faa1b-b9b5-49b5-1cc2-9514bdab0a64"), "Chemistry of Life — Biology Grade 10", "Chemistry of Life", 1, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("cd33959b-8ba9-cb96-3898-34c93816e98f"), "Newton's Laws (Applications) — Physics Grade 11", "Newton's Laws (Applications)", 2, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("cdd08660-6404-dd4b-4da7-f58d68ae3897"), "Literature: Drama Analysis — English Grade 11", "Literature: Drama Analysis", 6, new Guid("99b68af6-34fc-713c-884f-c018c6b17d72") },
                    { new Guid("d02c0f9a-e3f0-499e-744f-1555b82d3581"), "Literature: Poetry — English Grade 10", "Literature: Poetry", 4, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("d03460f3-f948-4426-bdbf-8c091519d492"), "Number Patterns — Mathematics Grade 10", "Number Patterns", 3, new Guid("f9d10be1-9d0d-14ec-37e4-3ab4dfc611f8") },
                    { new Guid("d4770125-312b-dd21-7945-d24510e977f5"), "Ideal Gases (Gas Laws) — Chemistry Grade 11", "Ideal Gases (Gas Laws)", 3, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("d577391d-8052-ee84-1167-f7a74473a137"), "Grammar and Sentence Structure — English Grade 10", "Grammar and Sentence Structure", 10, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("d57cb817-d7f3-efd6-aae8-ea320ccba361"), "GIS Applications — Geography Grade 11", "GIS Applications", 9, new Guid("629825b8-c2d2-7aee-d163-0be88495e272") },
                    { new Guid("d8eb0352-ad2a-b199-aa10-a4db9eb9e308"), "Animal Health and Diseases — Agriculture Grade 11", "Animal Health and Diseases", 5, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("db34d196-bee5-f55a-ac42-12b3644bca05"), "Agricultural Technology — Agriculture Grade 10", "Agricultural Technology", 10, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("ddeb7bb0-2796-b4f5-151b-55ad39bd4715"), "Electromagnetic Radiation — Physics Grade 10", "Electromagnetic Radiation", 7, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("df30a84d-9cb6-128b-745f-2a71a232f4ec"), "The Lithosphere (Mining, Energy) — Chemistry Grade 11", "The Lithosphere (Mining, Energy)", 7, new Guid("f71b0725-2ab4-36b8-c08a-d709a40e16f4") },
                    { new Guid("df945f07-f6fd-d786-9e3a-ca853df8eddf"), "States of Matter and Energy — Science Grade 10", "States of Matter and Energy", 3, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("df9b8207-2991-efd2-9864-5cd2fe8d486f"), "Development Geography (Global Issues) — Geography Grade 12", "Development Geography (Global Issues)", 5, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("e035f078-80d9-779b-beca-f25499bf63c4"), "Climate and Weather (Synoptic Charts) — Geography Grade 12", "Climate and Weather (Synoptic Charts)", 1, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("e0c6257b-a5de-6c40-b726-1aceac8e2f9a"), "Visual Literacy — English Grade 10", "Visual Literacy", 9, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("e4e71981-b951-05a5-37af-a4c1ab890849"), "History of Life on Earth — Biology Grade 10", "History of Life on Earth", 6, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("e5ab0208-183d-a059-c5ff-3c862c31674a"), "Electrostatics (Coulomb's Law) — Physics Grade 11", "Electrostatics (Coulomb's Law)", 8, new Guid("8c039749-e2dc-12fc-0757-80e63a219052") },
                    { new Guid("e5ea993b-9f92-26fd-9449-25fb109b2340"), "Electrostatics — Physics Grade 10", "Electrostatics", 8, new Guid("caea75ab-707a-872b-c92f-5500afab7afb") },
                    { new Guid("e624014c-6042-8ce8-ebc8-21db8dffceee"), "Agricultural Economics (Markets, Pricing) — Agriculture Grade 11", "Agricultural Economics (Markets, Pricing)", 6, new Guid("76324b9e-5fc2-7160-7522-932866c0a77a") },
                    { new Guid("e88283a1-61e0-ee45-46c6-693bdca5ee44"), "Grammar and Editing — English Grade 12", "Grammar and Editing", 9, new Guid("532a8048-546e-3d68-aa5a-14ae71eb6f9e") },
                    { new Guid("e951ea1c-8c9d-cc2b-6b45-c0916476bbd0"), "Chemical Change (Stoichiometry) — Science Grade 11", "Chemical Change (Stoichiometry)", 5, new Guid("82786b28-5a6d-b0b6-194d-ab131764064d") },
                    { new Guid("ea9ecd93-dd47-0dfc-805c-d22f1422484f"), "Exponents and Surds — Mathematics Grade 11", "Exponents and Surds", 1, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("eab93840-1960-cecc-c7c8-928cd373fc22"), "Trigonometry (Identities, Equations, Graphs) — Mathematics Grade 11", "Trigonometry (Identities, Equations, Graphs)", 6, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") },
                    { new Guid("ecb99fdd-8c39-dba9-8237-2e95125945f8"), "History of Life on Earth (Evolution) — Biology Grade 11", "History of Life on Earth (Evolution)", 2, new Guid("b862ea9a-d88a-6230-aeea-858a90d5a959") },
                    { new Guid("ecda0c05-a8b9-5244-b3dd-743ebfb367b0"), "Magnetism — Science Grade 10", "Magnetism", 8, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("ed750b69-66a5-81ae-b3ad-912c2e5e7ba3"), "Sustainable Farming Practices — Agriculture Grade 10", "Sustainable Farming Practices", 9, new Guid("e37e03f0-c9ec-1714-3ff7-408956f8a8c0") },
                    { new Guid("f1499460-3482-edd8-102f-374954f392ce"), "Matter and Classification — Science Grade 10", "Matter and Classification", 2, new Guid("082d128a-6ee4-2c5d-0efc-f9b0b9af4c23") },
                    { new Guid("f2171eb6-9c3d-271d-9302-bd5d1bf36cdb"), "Literature: Short Stories — English Grade 10", "Literature: Short Stories", 5, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("f36dfb07-4b90-fba9-f074-15dc22a07029"), "Plant and Animal Tissues — Biology Grade 10", "Plant and Animal Tissues", 4, new Guid("6aaf47ef-e5a0-d4af-70db-f2011158ef8e") },
                    { new Guid("f38c2c9d-e19b-8359-480c-67c3669b662f"), "Organic Chemistry (Reactions) — Chemistry Grade 12", "Organic Chemistry (Reactions)", 2, new Guid("26a00ceb-e8ed-7c0b-b245-a60b6107d495") },
                    { new Guid("f39fa9a8-b9fe-a19d-c71a-b9350eecf920"), "Food Security and Safety — Agriculture Grade 12", "Food Security and Safety", 9, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("f42371cf-aaf2-cfa0-994c-cc134d4fa9ad"), "Chemical Equilibrium — Science Grade 12", "Chemical Equilibrium", 9, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") },
                    { new Guid("f57ba5bd-1825-3bfd-616a-861b4eeae95d"), "Agro-Processing and Beneficiation — Agriculture Grade 12", "Agro-Processing and Beneficiation", 7, new Guid("69e6f790-ae65-2e87-f351-982b99d92bfe") },
                    { new Guid("f5ab6ef6-952a-47fa-81a2-1aa7aeb10694"), "Human Reproduction — Biology Grade 12", "Human Reproduction", 8, new Guid("06216eaa-a8e4-6710-e535-da8f3f9af209") },
                    { new Guid("f715ea05-b1ce-5c66-95f0-5a7ad3e6fed6"), "Map Work (Advanced Calculations) — Geography Grade 12", "Map Work (Advanced Calculations)", 3, new Guid("7a475323-7df5-f613-6449-fcfad4461d7c") },
                    { new Guid("f7e8d9d9-e003-667e-a79b-867704c18363"), "Transactional Writing (Letters, Reports) — English Grade 10", "Transactional Writing (Letters, Reports)", 8, new Guid("fdca9ad9-bc36-79d0-421e-38c118a6c3cf") },
                    { new Guid("f9f3697d-74dc-5cac-eaf3-9dc2d7c4e5f9"), "Organic Chemistry — Science Grade 12", "Organic Chemistry", 3, new Guid("89bdf99e-82a8-29b3-57d7-410b42d8dcf8") },
                    { new Guid("fe526bf2-961d-f568-89bf-28c52fc115a2"), "Analytical Geometry (Circles) — Mathematics Grade 11", "Analytical Geometry (Circles)", 7, new Guid("5295e6aa-8238-0794-a9ec-29deb0bef2aa") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_StudentId_Type",
                table: "Achievements",
                columns: new[] { "StudentId", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceTokens_StudentId",
                table: "DeviceTokens",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceTokens_Token",
                table: "DeviceTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentId_SubjectId",
                table: "Enrollments",
                columns: new[] { "StudentId", "SubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_SubjectId",
                table: "Enrollments",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_StudentId",
                table: "Flashcards",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_TopicId",
                table: "Flashcards",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_TopicId",
                table: "Notes",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TopicId",
                table: "Questions",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestions_QuizId",
                table: "QuizQuestions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_StudentId",
                table: "Quizzes",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_TopicId",
                table: "Quizzes",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_ScheduledAt_SentAt",
                table: "Reminders",
                columns: new[] { "ScheduledAt", "SentAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_StudentId",
                table: "Reminders",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentProgress_StudentId_SubjectId",
                table: "StudentProgress",
                columns: new[] { "StudentId", "SubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentProgress_SubjectId",
                table: "StudentProgress",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_PhoneNumber",
                table: "Students",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanItems_StudyPlanId",
                table: "StudyPlanItems",
                column: "StudyPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanItems_TopicId",
                table: "StudyPlanItems",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlans_StudentId",
                table: "StudyPlans",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlans_SubjectId",
                table: "StudyPlans",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_StudySessions_StudentId_StartedAt",
                table: "StudySessions",
                columns: new[] { "StudentId", "StartedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_StudySessions_SubjectId",
                table: "StudySessions",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_StudySessions_TopicId",
                table: "StudySessions",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_Name_Grade",
                table: "Subjects",
                columns: new[] { "Name", "Grade" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimetableEntries_StudentId",
                table: "TimetableEntries",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TimetableEntries_SubjectId",
                table: "TimetableEntries",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_SubjectId_Order",
                table: "Topics",
                columns: new[] { "SubjectId", "Order" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Achievements");

            migrationBuilder.DropTable(
                name: "DeviceTokens");

            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "Flashcards");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "QuizQuestions");

            migrationBuilder.DropTable(
                name: "Reminders");

            migrationBuilder.DropTable(
                name: "StudentProgress");

            migrationBuilder.DropTable(
                name: "StudyPlanItems");

            migrationBuilder.DropTable(
                name: "StudySessions");

            migrationBuilder.DropTable(
                name: "TimetableEntries");

            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.DropTable(
                name: "StudyPlans");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Subjects");
        }
    }
}
