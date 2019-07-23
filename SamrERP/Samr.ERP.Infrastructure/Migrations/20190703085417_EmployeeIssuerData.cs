using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class EmployeeIssuerData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PassportMvdName",
                table: "Employees",
                newName: "PassportIssuer");

            migrationBuilder.RenameColumn(
                name: "PassportDate",
                table: "Employees",
                newName: "PassportIssueDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PassportIssuer",
                table: "Employees",
                newName: "PassportMvdName");

            migrationBuilder.RenameColumn(
                name: "PassportIssueDate",
                table: "Employees",
                newName: "PassportDate");
        }
    }
}
