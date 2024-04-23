using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Proj.Migrations
{
    /// <inheritdoc />
    public partial class LaptopsEmployeeOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Laptop_Employee_EmployeeID",
                table: "Laptop");

            migrationBuilder.DropIndex(
                name: "IX_Laptop_EmployeeID",
                table: "Laptop");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeID",
                table: "Laptop",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Laptop_EmployeeID",
                table: "Laptop",
                column: "EmployeeID",
                unique: true,
                filter: "[EmployeeID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Laptop_Employee_EmployeeID",
                table: "Laptop",
                column: "EmployeeID",
                principalTable: "Employee",
                principalColumn: "EmployeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Laptop_Employee_EmployeeID",
                table: "Laptop");

            migrationBuilder.DropIndex(
                name: "IX_Laptop_EmployeeID",
                table: "Laptop");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeID",
                table: "Laptop",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Laptop_EmployeeID",
                table: "Laptop",
                column: "EmployeeID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Laptop_Employee_EmployeeID",
                table: "Laptop",
                column: "EmployeeID",
                principalTable: "Employee",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
