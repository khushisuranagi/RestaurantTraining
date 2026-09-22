using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantTraining.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddResourceQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResourceQuestionOptions",
                columns: table => new
                {
                    OptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceQuestionId = table.Column<int>(type: "int", nullable: false),
                    OptionText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceQuestionOptions", x => x.OptionId);
                });

            migrationBuilder.CreateTable(
                name: "ResourceQuestions",
                columns: table => new
                {
                    ResourceQuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceId = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceQuestions", x => x.ResourceQuestionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceQuestionOptions_ResourceQuestionId",
                table: "ResourceQuestionOptions",
                column: "ResourceQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceQuestions_ResourceId",
                table: "ResourceQuestions",
                column: "ResourceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceQuestionOptions");

            migrationBuilder.DropTable(
                name: "ResourceQuestions");
        }
    }
}
