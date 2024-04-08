using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace finalProject.Migrations
{
    /// <inheritdoc />
    public partial class v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_logs_Employees_emp_id",
                table: "logs");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ContractDate",
                table: "newEmployees",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Birthdate",
                table: "newEmployees",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_logs_newEmployees_emp_id",
                table: "logs",
                column: "emp_id",
                principalTable: "newEmployees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_logs_newEmployees_emp_id",
                table: "logs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ContractDate",
                table: "newEmployees",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthdate",
                table: "newEmployees",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    arrival = table.Column<TimeOnly>(type: "time", nullable: false),
                    departure = table.Column<TimeOnly>(type: "time", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_logs_Employees_emp_id",
                table: "logs",
                column: "emp_id",
                principalTable: "Employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
