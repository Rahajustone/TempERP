using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class UrlLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "UsefulLinks",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeLockReasonId",
                table: "EmployeeLockReasonLogs",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: new Guid("9a3fcddb-4680-4206-b712-4e07df82e354"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 7, 10, 19, 9, 393, DateTimeKind.Local).AddTicks(3200));

            migrationBuilder.UpdateData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 7, 10, 19, 9, 392, DateTimeKind.Local).AddTicks(2867));

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLockReasonLogs_EmployeeLockReasonId",
                table: "EmployeeLockReasonLogs",
                column: "EmployeeLockReasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeLockReasonLogs_EmployeeLockReasons_EmployeeLockReas~",
                table: "EmployeeLockReasonLogs",
                column: "EmployeeLockReasonId",
                principalTable: "EmployeeLockReasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeLockReasonLogs_EmployeeLockReasons_EmployeeLockReas~",
                table: "EmployeeLockReasonLogs");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeLockReasonLogs_EmployeeLockReasonId",
                table: "EmployeeLockReasonLogs");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "UsefulLinks");

            migrationBuilder.DropColumn(
                name: "EmployeeLockReasonId",
                table: "EmployeeLockReasonLogs");

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: new Guid("9a3fcddb-4680-4206-b712-4e07df82e354"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 6, 12, 43, 58, 475, DateTimeKind.Local).AddTicks(6252));

            migrationBuilder.UpdateData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 6, 12, 43, 58, 474, DateTimeKind.Local).AddTicks(2836));
        }
    }
}
