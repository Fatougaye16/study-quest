using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyQuest.API.Migrations
{
    /// <inheritdoc />
    public partial class fluentvalidation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CachedAIContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentType = table.Column<int>(type: "integer", nullable: false),
                    TopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    InputHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ResponseJson = table.Column<string>(type: "text", nullable: false),
                    StudentGrade = table.Column<int>(type: "integer", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CachedAIContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CachedAIContents_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CachedDownloads",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentType = table.Column<int>(type: "integer", nullable: false),
                    SourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    PdfData = table.Column<byte[]>(type: "bytea", nullable: false),
                    ContentHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CachedDownloads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PastPapers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    ExamType = table.Column<int>(type: "integer", nullable: false),
                    PaperNumber = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CreatedByStudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PastPapers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PastPapers_Students_CreatedByStudentId",
                        column: x => x.CreatedByStudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PastPapers_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PastQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PastPaperId = table.Column<Guid>(type: "uuid", nullable: false),
                    TopicId = table.Column<Guid>(type: "uuid", nullable: true),
                    QuestionNumber = table.Column<int>(type: "integer", nullable: false),
                    QuestionText = table.Column<string>(type: "text", nullable: false),
                    AnswerText = table.Column<string>(type: "text", nullable: true),
                    Marks = table.Column<int>(type: "integer", nullable: true),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Difficulty = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PastQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PastQuestions_PastPapers_PastPaperId",
                        column: x => x.PastPaperId,
                        principalTable: "PastPapers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PastQuestions_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CachedAIContents_ContentType_TopicId_InputHash",
                table: "CachedAIContents",
                columns: new[] { "ContentType", "TopicId", "InputHash" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CachedAIContents_TopicId",
                table: "CachedAIContents",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_CachedDownloads_ContentType_SourceId",
                table: "CachedDownloads",
                columns: new[] { "ContentType", "SourceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PastPapers_CreatedByStudentId",
                table: "PastPapers",
                column: "CreatedByStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_PastPapers_SubjectId_Year_ExamType_PaperNumber",
                table: "PastPapers",
                columns: new[] { "SubjectId", "Year", "ExamType", "PaperNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PastQuestions_PastPaperId",
                table: "PastQuestions",
                column: "PastPaperId");

            migrationBuilder.CreateIndex(
                name: "IX_PastQuestions_TopicId",
                table: "PastQuestions",
                column: "TopicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CachedAIContents");

            migrationBuilder.DropTable(
                name: "CachedDownloads");

            migrationBuilder.DropTable(
                name: "PastQuestions");

            migrationBuilder.DropTable(
                name: "PastPapers");
        }
    }
}
