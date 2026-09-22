using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantTraining.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddResourceSummaries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResourceSummaries",
                columns: table => new
                {
                    ResourceSummaryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceId = table.Column<int>(type: "int", nullable: false),
                    SummaryText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceSummaries", x => x.ResourceSummaryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceSummaries_ResourceId",
                table: "ResourceSummaries",
                column: "ResourceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceSummaries");
        }
    }
}
