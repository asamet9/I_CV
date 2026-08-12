using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICV.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCvAnalysis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CvAnalysis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CvId = table.Column<int>(type: "int", nullable: false),
                    ProfessionId = table.Column<int>(type: "int", nullable: false),
                    MatchedSkillCount = table.Column<int>(type: "int", nullable: false),
                    MissingSkillCount = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CvAnalysis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CvAnalysis_Cv_CvId",
                        column: x => x.CvId,
                        principalTable: "Cv",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CvAnalysis_Profession_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Profession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CvAnalysis_CvId",
                table: "CvAnalysis",
                column: "CvId");

            migrationBuilder.CreateIndex(
                name: "IX_CvAnalysis_ProfessionId",
                table: "CvAnalysis",
                column: "ProfessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CvAnalysis");
        }
    }
}
