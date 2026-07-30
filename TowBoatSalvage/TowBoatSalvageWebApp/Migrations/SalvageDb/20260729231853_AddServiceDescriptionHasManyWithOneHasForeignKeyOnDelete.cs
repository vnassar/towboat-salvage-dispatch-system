using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TowBoatSalvageWebApp.Migrations.SalvageDb
{
    /// <inheritdoc />
    public partial class AddServiceDescriptionHasManyWithOneHasForeignKeyOnDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceDescription",
                table: "ServiceDescription");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ServiceDescription",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "Honda500HrId",
                table: "ServiceDescription",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceDescription",
                table: "ServiceDescription",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceDescription_Honda500HrId",
                table: "ServiceDescription",
                column: "Honda500HrId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceDescription",
                table: "ServiceDescription");

            migrationBuilder.DropIndex(
                name: "IX_ServiceDescription_Honda500HrId",
                table: "ServiceDescription");

            migrationBuilder.AlterColumn<int>(
                name: "Honda500HrId",
                table: "ServiceDescription",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ServiceDescription",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceDescription",
                table: "ServiceDescription",
                columns: new[] { "Honda500HrId", "Id" });
        }
    }
}
