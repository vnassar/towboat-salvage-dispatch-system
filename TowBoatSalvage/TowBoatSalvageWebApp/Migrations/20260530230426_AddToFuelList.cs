using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TowBoatSalvageWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddToFuelList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AddToFuelList",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddToFuelList",
                table: "AspNetUsers");
        }
    }
}
