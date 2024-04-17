using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Proj.Migrations
{
    /// <inheritdoc />
    public partial class SecondSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "YearsAtCompany",
                table: "Employee",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeID",
                keyValue: 1001,
                column: "YearsAtCompany",
                value: 0.5);

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "EmployeeID", "CurrentProjects", "EmployeeName", "JobTitle", "YearsAtCompany" },
                values: new object[] { 1002, "[\"Twidling Thumbs\"]", "Hoa Nguyen", "Student Developer", 0.5 });

            migrationBuilder.InsertData(
                table: "Laptop",
                columns: new[] { "LaptopID", "EmployeeID", "LaptopName" },
                values: new object[] { 1002, 1002, "Hoa's Laptop" });

            migrationBuilder.InsertData(
                table: "OfficeEmployee",
                columns: new[] { "EmployeesID", "OfficesID" },
                values: new object[] { 1002, 1001 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Laptop",
                keyColumn: "LaptopID",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "OfficeEmployee",
                keyColumns: new[] { "EmployeesID", "OfficesID" },
                keyValues: new object[] { 1002, 1001 });

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "EmployeeID",
                keyValue: 1002);

            migrationBuilder.AlterColumn<int>(
                name: "YearsAtCompany",
                table: "Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeID",
                keyValue: 1001,
                column: "YearsAtCompany",
                value: 0);
        }
    }
}
