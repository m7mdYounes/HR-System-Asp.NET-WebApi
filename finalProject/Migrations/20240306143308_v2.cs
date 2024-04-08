using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace finalProject.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    arrival = table.Column<TimeOnly>(type: "time", nullable: false),
                    departure = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    day = table.Column<DateOnly>(type: "date", nullable: false),
                    day_arrival = table.Column<TimeOnly>(type: "time", nullable: false),
                    day_departure = table.Column<TimeOnly>(type: "time", nullable: false),
                    emp_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_logs_Employees_emp_id",
                        column: x => x.emp_id,
                        principalTable: "Employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_logs_emp_id",
                table: "logs",
                column: "emp_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "logs");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
