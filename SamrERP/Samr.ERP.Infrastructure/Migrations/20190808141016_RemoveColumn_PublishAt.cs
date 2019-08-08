using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class RemoveColumn_PublishAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishAt",
                table: "News");

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: new Guid("9a3fcddb-4680-4206-b712-4e07df82e354"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 8, 19, 10, 16, 223, DateTimeKind.Local).AddTicks(6905));

            migrationBuilder.UpdateData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 8, 19, 10, 16, 222, DateTimeKind.Local).AddTicks(5038));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublishAt",
                table: "News",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: new Guid("9a3fcddb-4680-4206-b712-4e07df82e354"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 8, 14, 30, 29, 207, DateTimeKind.Local).AddTicks(7254));

            migrationBuilder.UpdateData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 8, 14, 30, 29, 206, DateTimeKind.Local).AddTicks(5478));
        }
    }
}
