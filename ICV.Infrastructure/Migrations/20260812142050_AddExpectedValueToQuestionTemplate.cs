using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICV.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExpectedValueToQuestionTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExpectedValue",
                table: "QuestionTemplate",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedValue",
                table: "QuestionTemplate");
        }
    }
}
