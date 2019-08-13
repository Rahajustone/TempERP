using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class NullableFieldNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReadDate",
                table: "Notifications",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: new Guid("9a3fcddb-4680-4206-b712-4e07df82e354"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 13, 17, 32, 0, 362, DateTimeKind.Local).AddTicks(7217));

            migrationBuilder.UpdateData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 13, 17, 32, 0, 361, DateTimeKind.Local).AddTicks(4487));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReadDate",
                table: "Notifications",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: new Guid("9a3fcddb-4680-4206-b712-4e07df82e354"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 13, 14, 24, 56, 3, DateTimeKind.Local).AddTicks(9124));

            migrationBuilder.UpdateData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 13, 14, 24, 56, 2, DateTimeKind.Local).AddTicks(5605));
        }
    }
}
