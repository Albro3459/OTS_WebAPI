using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Proj.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    YearsAtCompany = table.Column<double>(type: "float", nullable: true),
                    CurrentProjects = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeID);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    RegionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.RegionID);
                });

            migrationBuilder.CreateTable(
                name: "Laptop",
                columns: table => new
                {
                    LaptopID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LaptopName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmployeeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Laptop", x => x.LaptopID);
                    table.ForeignKey(
                        name: "FK_Laptop_Employee_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "Office",
                columns: table => new
                {
                    OfficeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Office", x => x.OfficeID);
                    table.ForeignKey(
                        name: "FK_Office_Region_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Region",
                        principalColumn: "RegionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfficeEmployee",
                columns: table => new
                {
                    OfficesID = table.Column<int>(type: "int", nullable: false),
                    EmployeesID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeEmployee", x => new { x.OfficesID, x.EmployeesID });
                    table.ForeignKey(
                        name: "FK_OfficeEmployee_Employee_EmployeesID",
                        column: x => x.EmployeesID,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficeEmployee_Office_OfficesID",
                        column: x => x.OfficesID,
                        principalTable: "Office",
                        principalColumn: "OfficeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Laptop_EmployeeID",
                table: "Laptop",
                column: "EmployeeID",
                unique: true,
                filter: "[EmployeeID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Office_RegionID",
                table: "Office",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeEmployee_EmployeesID",
                table: "OfficeEmployee",
                column: "EmployeesID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Laptop");

            migrationBuilder.DropTable(
                name: "OfficeEmployee");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Office");

            migrationBuilder.DropTable(
                name: "Region");
        }
    }
}
