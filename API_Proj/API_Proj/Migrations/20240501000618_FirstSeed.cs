using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API_Proj.Migrations
{
    /// <inheritdoc />
    public partial class FirstSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "EmployeeID", "CurrentProjects", "EmployeeName", "IsDeleted", "JobTitle", "YearsAtCompany" },
                values: new object[,]
                {
                    { 1001, "[\"Api Project\"]", "Alex Brodsky", false, "Student Developer", 0.5 },
                    { 1002, "[\"Twidling Thumbs\"]", "Hoa Nguyen", false, "Student Developer", 0.5 }
                });

            migrationBuilder.InsertData(
                table: "Region",
                columns: new[] { "RegionID", "IsDeleted", "RegionName" },
                values: new object[,]
                {
                    { 1001, false, "South West" },
                    { 1002, false, "South" }
                });

            migrationBuilder.InsertData(
                table: "Laptop",
                columns: new[] { "LaptopID", "EmployeeID", "IsDeleted", "LaptopName" },
                values: new object[,]
                {
                    { 1001, 1001, false, "Brodsky's Laptop" },
                    { 1002, 1002, false, "Hoa's Laptop" }
                });

            migrationBuilder.InsertData(
                table: "Office",
                columns: new[] { "OfficeID", "IsDeleted", "OfficeName", "RegionID" },
                values: new object[,]
                {
                    { 1001, false, "Galvez Building", 1001 },
                    { 1002, false, "Deloitte Austin", 1002 }
                });

            migrationBuilder.InsertData(
                table: "OfficeEmployee",
                columns: new[] { "EmployeesID", "OfficesID" },
                values: new object[,]
                {
                    { 1001, 1001 },
                    { 1002, 1001 },
                    { 1001, 1002 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Laptop",
                keyColumn: "LaptopID",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "Laptop",
                keyColumn: "LaptopID",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "OfficeEmployee",
                keyColumns: new[] { "EmployeesID", "OfficesID" },
                keyValues: new object[] { 1001, 1001 });

            migrationBuilder.DeleteData(
                table: "OfficeEmployee",
                keyColumns: new[] { "EmployeesID", "OfficesID" },
                keyValues: new object[] { 1002, 1001 });

            migrationBuilder.DeleteData(
                table: "OfficeEmployee",
                keyColumns: new[] { "EmployeesID", "OfficesID" },
                keyValues: new object[] { 1001, 1002 });

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "EmployeeID",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "EmployeeID",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "Office",
                keyColumn: "OfficeID",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "Office",
                keyColumn: "OfficeID",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "Region",
                keyColumn: "RegionID",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "Region",
                keyColumn: "RegionID",
                keyValue: 1002);
        }
    }
}
