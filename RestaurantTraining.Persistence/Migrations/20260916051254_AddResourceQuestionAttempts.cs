using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantTraining.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddResourceQuestionAttempts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResourceQuestionAttempts",
                columns: table => new
                {
                    AttemptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ResourceId = table.Column<int>(type: "int", nullable: false),
                    PointsAwarded = table.Column<int>(type: "int", nullable: false),
                    AnsweredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceQuestionAttempts", x => x.AttemptId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceQuestionAttempts_UserId_ResourceId",
                table: "ResourceQuestionAttempts",
                columns: new[] { "UserId", "ResourceId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceQuestionAttempts");
        }
    }
}
