using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TowBoatSalvageWebApp.Migrations.SalvageDb
{
    /// <inheritdoc />
    public partial class AddVesselInspectionRedo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VesselInspection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BoatNumber = table.Column<string>(type: "TEXT", nullable: false),
                    CompletedBy = table.Column<string>(type: "TEXT", nullable: false),
                    DateOfInspection = table.Column<DateTime>(type: "TEXT", nullable: true),
                    bHasBeenDownloaded = table.Column<bool>(type: "INTEGER", nullable: false),
                    bIsResolved = table.Column<bool>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VesselInspection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceDescriptionVesselInspection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    bServiceCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    DateForThisItem = table.Column<DateTime>(type: "TEXT", nullable: true),
                    bThisItemRequiresDate = table.Column<bool>(type: "INTEGER", nullable: false),
                    FirstRecording = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SecondRecording = table.Column<DateTime>(type: "TEXT", nullable: true),
                    VesselInspectionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceDescriptionVesselInspection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceDescriptionVesselInspection_VesselInspection_VesselInspectionId",
                        column: x => x.VesselInspectionId,
                        principalTable: "VesselInspection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceDescriptionVesselInspection_VesselInspectionId",
                table: "ServiceDescriptionVesselInspection",
                column: "VesselInspectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceDescriptionVesselInspection");

            migrationBuilder.DropTable(
                name: "VesselInspection");
        }
    }
}
