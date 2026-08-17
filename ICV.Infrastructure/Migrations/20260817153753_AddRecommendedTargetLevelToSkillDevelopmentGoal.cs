using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICV.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRecommendedTargetLevelToSkillDevelopmentGoal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecommendedTargetLevel",
                table: "SkillDevelopmentGoal",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecommendedTargetLevel",
                table: "SkillDevelopmentGoal");
        }
    }
}