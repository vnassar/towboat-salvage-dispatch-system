using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TowBoatSalvageWebApp.Migrations.SalvageDb
{
    /// <inheritdoc />
    public partial class AddHonda500Hr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Honda500HrServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BoatNumber = table.Column<string>(type: "TEXT", nullable: false),
                    EngineHours1 = table.Column<int>(type: "INTEGER", nullable: false),
                    EngineHours2 = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCompleted = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompletedBy = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Honda500HrServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceDescription",
                columns: table => new
                {
                    Honda500HrId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    bServiceCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceDescription", x => new { x.Honda500HrId, x.Id });
                    table.ForeignKey(
                        name: "FK_ServiceDescription_Honda500HrServices_Honda500HrId",
                        column: x => x.Honda500HrId,
                        principalTable: "Honda500HrServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceDescription");

            migrationBuilder.DropTable(
                name: "Honda500HrServices");
        }
    }
}
