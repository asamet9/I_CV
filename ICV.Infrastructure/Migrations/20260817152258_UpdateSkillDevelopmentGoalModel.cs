using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICV.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSkillDevelopmentGoalModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ---------------------------------------------------------
            // SKILL SUGGESTION
            // ---------------------------------------------------------

            migrationBuilder.AddColumn<int>(
                name: "RecommendedTargetLevel",
                table: "SkillSuggestion",
                type: "int",
                nullable: false,
                defaultValue: 1);


            // ---------------------------------------------------------
            // SKILL DEVELOPMENT GOAL
            // ---------------------------------------------------------

            // Eski modelden kalan kolon artık kullanılmıyor.
            migrationBuilder.DropColumn(
                name: "TargetDate",
                table: "SkillDevelopmentGoal");

            // Eski WeeklyHours alanı artık kullanılmıyor.
            migrationBuilder.DropColumn(
                name: "WeeklyHours",
                table: "SkillDevelopmentGoal");

            // Yeni gelişim süresi.
            migrationBuilder.AddColumn<int>(
                name: "PreferredDuration",
                table: "SkillDevelopmentGoal",
                type: "int",
                nullable: false,
                defaultValue: 1);

            // Ücretli kurs tercihi.
            migrationBuilder.AddColumn<bool>(
                name: "WantsPaidCourse",
                table: "SkillDevelopmentGoal",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // Sertifika tercihi.
            migrationBuilder.AddColumn<bool>(
                name: "WantsCertificate",
                table: "SkillDevelopmentGoal",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // ---------------------------------------------------------
            // SKILL SUGGESTION
            // ---------------------------------------------------------

            migrationBuilder.DropColumn(
                name: "RecommendedTargetLevel",
                table: "SkillSuggestion");


            // ---------------------------------------------------------
            // SKILL DEVELOPMENT GOAL
            // ---------------------------------------------------------

            migrationBuilder.DropColumn(
                name: "PreferredDuration",
                table: "SkillDevelopmentGoal");

            migrationBuilder.DropColumn(
                name: "WantsPaidCourse",
                table: "SkillDevelopmentGoal");

            migrationBuilder.DropColumn(
                name: "WantsCertificate",
                table: "SkillDevelopmentGoal");

            migrationBuilder.AddColumn<int>(
                name: "WeeklyHours",
                table: "SkillDevelopmentGoal",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TargetDate",
                table: "SkillDevelopmentGoal",
                type: "datetime2",
                nullable: true);
        }
    }
}