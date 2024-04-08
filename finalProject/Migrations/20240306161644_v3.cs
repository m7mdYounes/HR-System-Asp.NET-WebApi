using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace finalProject.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeeklyLeaveDays",
                table: "settings");

            migrationBuilder.AddColumn<int>(
                name: "HolidayDayOne",
                table: "settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HolidayDayTwo",
                table: "settings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HolidayDayOne",
                table: "settings");

            migrationBuilder.DropColumn(
                name: "HolidayDayTwo",
                table: "settings");

            migrationBuilder.AddColumn<string>(
                name: "WeeklyLeaveDays",
                table: "settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
