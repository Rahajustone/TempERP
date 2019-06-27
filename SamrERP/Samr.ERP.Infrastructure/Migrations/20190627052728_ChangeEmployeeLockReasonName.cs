using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class ChangeEmployeeLockReasonName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeLockTypes_EmployeeLockReason",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "EmployeeLockReason",
                table: "Employees",
                newName: "EmployeeLockTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_EmployeeLockReason",
                table: "Employees",
                newName: "IX_Employees_EmployeeLockTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeLockTypes_EmployeeLockTypeId",
                table: "Employees",
                column: "EmployeeLockTypeId",
                principalTable: "EmployeeLockTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeLockTypes_EmployeeLockTypeId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "EmployeeLockTypeId",
                table: "Employees",
                newName: "EmployeeLockReason");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_EmployeeLockTypeId",
                table: "Employees",
                newName: "IX_Employees_EmployeeLockReason");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeLockTypes_EmployeeLockReason",
                table: "Employees",
                column: "EmployeeLockReason",
                principalTable: "EmployeeLockTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
