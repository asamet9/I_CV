using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICV.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSkillDevelopmentGoalToCourseRecommendation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseRecommendation_Course_CourseId",
                table: "CourseRecommendation");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseRecommendation_SkillSuggestion_SkillSuggestionId",
                table: "CourseRecommendation");

            migrationBuilder.DropIndex(
                name: "IX_CourseRecommendation_SkillSuggestionId",
                table: "CourseRecommendation");

            migrationBuilder.DropColumn(
                name: "DurationWeeks",
                table: "CourseRecommendation");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "CourseRecommendation");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "CourseRecommendation");

            migrationBuilder.DropColumn(
                name: "Provider",
                table: "CourseRecommendation");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "CourseRecommendation");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "CourseRecommendation");

            migrationBuilder.RenameColumn(
                name: "SkillSuggestionId",
                table: "CourseRecommendation",
                newName: "SkillDevelopmentGoalId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRecommendation_SkillDevelopmentGoalId_CourseId",
                table: "CourseRecommendation",
                columns: new[] { "SkillDevelopmentGoalId", "CourseId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseRecommendation_Course_CourseId",
                table: "CourseRecommendation",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseRecommendation_SkillDevelopmentGoal_SkillDevelopmentGoalId",
                table: "CourseRecommendation",
                column: "SkillDevelopmentGoalId",
                principalTable: "SkillDevelopmentGoal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseRecommendation_Course_CourseId",
                table: "CourseRecommendation");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseRecommendation_SkillDevelopmentGoal_SkillDevelopmentGoalId",
                table: "CourseRecommendation");

            migrationBuilder.DropIndex(
                name: "IX_CourseRecommendation_SkillDevelopmentGoalId_CourseId",
                table: "CourseRecommendation");

            migrationBuilder.RenameColumn(
                name: "SkillDevelopmentGoalId",
                table: "CourseRecommendation",
                newName: "SkillSuggestionId");

            migrationBuilder.AddColumn<int>(
                name: "DurationWeeks",
                table: "CourseRecommendation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "CourseRecommendation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "CourseRecommendation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "CourseRecommendation",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CourseRecommendation",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "CourseRecommendation",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRecommendation_SkillSuggestionId",
                table: "CourseRecommendation",
                column: "SkillSuggestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseRecommendation_Course_CourseId",
                table: "CourseRecommendation",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseRecommendation_SkillSuggestion_SkillSuggestionId",
                table: "CourseRecommendation",
                column: "SkillSuggestionId",
                principalTable: "SkillSuggestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
