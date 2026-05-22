using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TowBoatSalvageWebApp.Migrations.SalvageDb
{
    /// <inheritdoc />
    public partial class MailGunEventTrackingAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EmailAcceptedAtUtc",
                table: "DocumentSignatureRequests",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailDeliveredAtUtc",
                table: "DocumentSignatureRequests",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailFailedAtUtc",
                table: "DocumentSignatureRequests",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailFailureReason",
                table: "DocumentSignatureRequests",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailLastEvent",
                table: "DocumentSignatureRequests",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailOpenedAtUtc",
                table: "DocumentSignatureRequests",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAcceptedAtUtc",
                table: "DocumentSignatureRequests");

            migrationBuilder.DropColumn(
                name: "EmailDeliveredAtUtc",
                table: "DocumentSignatureRequests");

            migrationBuilder.DropColumn(
                name: "EmailFailedAtUtc",
                table: "DocumentSignatureRequests");

            migrationBuilder.DropColumn(
                name: "EmailFailureReason",
                table: "DocumentSignatureRequests");

            migrationBuilder.DropColumn(
                name: "EmailLastEvent",
                table: "DocumentSignatureRequests");

            migrationBuilder.DropColumn(
                name: "EmailOpenedAtUtc",
                table: "DocumentSignatureRequests");
        }
    }
}
