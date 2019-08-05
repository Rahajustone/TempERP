using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class ChangeFieldToTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShortDescription",
                table: "FileArchives",
                newName: "Title");

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: new Guid("9a3fcddb-4680-4206-b712-4e07df82e354"),
                columns: new[] { "CreatedAt", "EnabledSSL" },
                values: new object[] { new DateTime(2019, 8, 5, 13, 11, 52, 989, DateTimeKind.Local).AddTicks(9475), true });

            migrationBuilder.UpdateData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 5, 13, 11, 52, 988, DateTimeKind.Local).AddTicks(4144));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "FileArchives",
                newName: "ShortDescription");

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: new Guid("9a3fcddb-4680-4206-b712-4e07df82e354"),
                columns: new[] { "CreatedAt", "EnabledSSL" },
                values: new object[] { new DateTime(2019, 8, 3, 12, 21, 20, 228, DateTimeKind.Local).AddTicks(2473), false });

            migrationBuilder.UpdateData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 3, 12, 21, 20, 226, DateTimeKind.Local).AddTicks(9928));
        }
    }
}
