using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class AddEmployeeReason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeLockTypes_EmployeeLockTypeId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "EmployeeLockTypes");

            migrationBuilder.RenameColumn(
                name: "EmployeeLockTypeId",
                table: "Employees",
                newName: "EmployeeLockReasonId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_EmployeeLockTypeId",
                table: "Employees",
                newName: "IX_Employees_EmployeeLockReasonId");

            migrationBuilder.CreateTable(
                name: "EmployeeLockReasons",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLockReasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeLockReasons_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLockReasons_CreatedUserId",
                table: "EmployeeLockReasons",
                column: "CreatedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeLockReasons_EmployeeLockReasonId",
                table: "Employees",
                column: "EmployeeLockReasonId",
                principalTable: "EmployeeLockReasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeLockReasons_EmployeeLockReasonId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "EmployeeLockReasons");

            migrationBuilder.RenameColumn(
                name: "EmployeeLockReasonId",
                table: "Employees",
                newName: "EmployeeLockTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_EmployeeLockReasonId",
                table: "Employees",
                newName: "IX_Employees_EmployeeLockTypeId");

            migrationBuilder.CreateTable(
                name: "EmployeeLockTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLockTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeLockTypes_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLockTypes_CreatedUserId",
                table: "EmployeeLockTypes",
                column: "CreatedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeLockTypes_EmployeeLockTypeId",
                table: "Employees",
                column: "EmployeeLockTypeId",
                principalTable: "EmployeeLockTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
