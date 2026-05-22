using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TowBoatSalvageWebApp.Migrations.SalvageDb
{
    /// <inheritdoc />
    public partial class AddCreditCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreditCardId",
                table: "DocumentSignatureRequests",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CreditCard",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardHolderName = table.Column<string>(type: "TEXT", nullable: true),
                    CardNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Expiration = table.Column<string>(type: "TEXT", nullable: true),
                    CVV = table.Column<string>(type: "TEXT", nullable: true),
                    AuthorizedAmount = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCard", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSignatureRequests_CreditCardId",
                table: "DocumentSignatureRequests",
                column: "CreditCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentSignatureRequests_CreditCard_CreditCardId",
                table: "DocumentSignatureRequests",
                column: "CreditCardId",
                principalTable: "CreditCard",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentSignatureRequests_CreditCard_CreditCardId",
                table: "DocumentSignatureRequests");

            migrationBuilder.DropTable(
                name: "CreditCard");

            migrationBuilder.DropIndex(
                name: "IX_DocumentSignatureRequests_CreditCardId",
                table: "DocumentSignatureRequests");

            migrationBuilder.DropColumn(
                name: "CreditCardId",
                table: "DocumentSignatureRequests");
        }
    }
}
