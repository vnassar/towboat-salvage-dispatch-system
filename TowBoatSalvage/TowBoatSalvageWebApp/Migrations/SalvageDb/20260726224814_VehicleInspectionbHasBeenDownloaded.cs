using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TowBoatSalvageWebApp.Migrations.SalvageDb
{
    /// <inheritdoc />
    public partial class VehicleInspectionbHasBeenDownloaded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "bHasBeenDownloaded",
                table: "VehicleInspection",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bHasBeenDownloaded",
                table: "VehicleInspection");
        }
    }
}
