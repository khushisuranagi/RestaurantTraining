using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantTraining.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleCertificateSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModuleCertificateSettings",
                columns: table => new
                {
                    SettingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MinimumPassingScore = table.Column<int>(type: "int", nullable: false),
                    Template = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IssuerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleCertificateSettings", x => x.SettingId);
                    table.ForeignKey(
                        name: "FK_ModuleCertificateSettings_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModuleCertificateSettings_ModuleId",
                table: "ModuleCertificateSettings",
                column: "ModuleId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModuleCertificateSettings");
        }
    }
}
