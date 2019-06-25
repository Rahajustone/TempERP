using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class FixEployeeLockentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "EmployeeLock");

            migrationBuilder.RenameColumn(
                name: "Updated",
                table: "EmployeeLock",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "EmployeeLock",
                newName: "IsActive");

            migrationBuilder.AddColumn<Guid>(
                name: "CreateUserId",
                table: "EmployeeLock",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "EmployeeLock");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "EmployeeLock",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "EmployeeLock",
                newName: "Updated");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "EmployeeLock",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
