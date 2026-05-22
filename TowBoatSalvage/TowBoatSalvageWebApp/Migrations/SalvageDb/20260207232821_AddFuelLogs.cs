using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TowBoatSalvageWebApp.Migrations.SalvageDb
{
    /// <inheritdoc />
    public partial class AddFuelLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FuelLogs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BoatName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CrewMember = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LogDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Engine1Hours = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Engine2Hours = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Fuel1 = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Fuel2 = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelLogs", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuelLogs_BoatName_LogDate",
                table: "FuelLogs",
                columns: new[] { "BoatName", "LogDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuelLogs");
        }
    }
}
