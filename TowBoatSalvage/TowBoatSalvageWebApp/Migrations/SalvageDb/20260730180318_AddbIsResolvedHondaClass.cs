using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TowBoatSalvageWebApp.Migrations.SalvageDb
{
    /// <inheritdoc />
    public partial class AddbIsResolvedHondaClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "bIsResolved",
                table: "Honda500HrServices",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bIsResolved",
                table: "Honda500HrServices");
        }
    }
}
