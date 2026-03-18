using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccidentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Accidents",
                newName: "AccidentDate");

            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "Accidents",
                newName: "DamageCost");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Accidents",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Accidents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DamageCost",
                table: "Accidents",
                newName: "Cost");

            migrationBuilder.RenameColumn(
                name: "AccidentDate",
                table: "Accidents",
                newName: "Date");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Accidents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Accidents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
