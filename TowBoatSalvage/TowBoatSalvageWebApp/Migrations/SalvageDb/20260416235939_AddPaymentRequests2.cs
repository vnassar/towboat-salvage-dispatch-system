using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TowBoatSalvageWebApp.Migrations.SalvageDb
{
    /// <inheritdoc />
    public partial class AddPaymentRequests2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CaptainEmail = table.Column<string>(type: "TEXT", nullable: false),
                    CaptainName = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerName = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerEmail = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerPhone = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    StripePaymentLinkId = table.Column<string>(type: "TEXT", nullable: false),
                    StripePaymentLinkUrl = table.Column<string>(type: "TEXT", nullable: false),
                    StripeSessionId = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PaidAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EmailAcceptedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EmailDeliveredAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EmailOpenedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EmailFailedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EmailLastEvent = table.Column<string>(type: "TEXT", nullable: true),
                    EmailFailureReason = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRequests", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequests_StripePaymentLinkId",
                table: "PaymentRequests",
                column: "StripePaymentLinkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentRequests");
        }
    }
}
