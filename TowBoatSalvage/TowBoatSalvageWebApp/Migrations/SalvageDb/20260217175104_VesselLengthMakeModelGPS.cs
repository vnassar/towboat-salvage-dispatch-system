using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TowBoatSalvageWebApp.Migrations.SalvageDb
{
    /// <inheritdoc />
    public partial class VesselLengthMakeModelGPS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BoatRegistration",
                table: "DocumentSignatureRequests",
                newName: "VesselName");

            migrationBuilder.AddColumn<string>(
                name: "GPS",
                table: "DocumentSignatureRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VesselLength",
                table: "DocumentSignatureRequests",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VesselMakeModel",
                table: "DocumentSignatureRequests",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GPS",
                table: "DocumentSignatureRequests");

            migrationBuilder.DropColumn(
                name: "VesselLength",
                table: "DocumentSignatureRequests");

            migrationBuilder.DropColumn(
                name: "VesselMakeModel",
                table: "DocumentSignatureRequests");

            migrationBuilder.RenameColumn(
                name: "VesselName",
                table: "DocumentSignatureRequests",
                newName: "BoatRegistration");
        }
    }
}
