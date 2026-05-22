using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TowBoatSalvageWebApp.Migrations.SalvageDb
{
    /// <inheritdoc />
    public partial class AddOriginAndDestinationForForms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnaccompaniedDestination",
                table: "DocumentSignatureRequests",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnaccompaniedOrigin",
                table: "DocumentSignatureRequests",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnaccompaniedDestination",
                table: "DocumentSignatureRequests");

            migrationBuilder.DropColumn(
                name: "UnaccompaniedOrigin",
                table: "DocumentSignatureRequests");
        }
    }
}
