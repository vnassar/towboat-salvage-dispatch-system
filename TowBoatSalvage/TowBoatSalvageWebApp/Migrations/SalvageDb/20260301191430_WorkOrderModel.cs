using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TowBoatSalvageWebApp.Migrations.SalvageDb
{
    /// <inheritdoc />
    public partial class WorkOrderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VesselName = table.Column<string>(type: "TEXT", nullable: false),
                    RequestDateDisplay = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Engine1Hours = table.Column<string>(type: "TEXT", nullable: false),
                    Engine2Hours = table.Column<string>(type: "TEXT", nullable: false),
                    ReportedIssues = table.Column<string>(type: "TEXT", nullable: false),
                    IsAddingCorrection = table.Column<bool>(type: "INTEGER", nullable: false),
                    CorrectionNotes = table.Column<string>(type: "TEXT", nullable: false),
                    IsResolved = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrder", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkOrder");
        }
    }
}
