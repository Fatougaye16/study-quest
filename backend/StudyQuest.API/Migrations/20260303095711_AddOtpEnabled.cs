using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyQuest.API.Migrations
{
    /// <inheritdoc />
    public partial class AddOtpEnabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOtpEnabled",
                table: "Students",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOtpEnabled",
                table: "Students");
        }
    }
}
