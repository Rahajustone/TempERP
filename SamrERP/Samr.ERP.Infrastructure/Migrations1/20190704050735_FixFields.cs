using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class FixFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Nationalities_PassportNationalityId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "PassportNationalityId",
                table: "Employees",
                newName: "NationalityId");

            migrationBuilder.RenameColumn(
                name: "AddressFact",
                table: "Employees",
                newName: "FactualAddress");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_PassportNationalityId",
                table: "Employees",
                newName: "IX_Employees_NationalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Nationalities_NationalityId",
                table: "Employees",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Nationalities_NationalityId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "NationalityId",
                table: "Employees",
                newName: "PassportNationalityId");

            migrationBuilder.RenameColumn(
                name: "FactualAddress",
                table: "Employees",
                newName: "AddressFact");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_NationalityId",
                table: "Employees",
                newName: "IX_Employees_PassportNationalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Nationalities_PassportNationalityId",
                table: "Employees",
                column: "PassportNationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
