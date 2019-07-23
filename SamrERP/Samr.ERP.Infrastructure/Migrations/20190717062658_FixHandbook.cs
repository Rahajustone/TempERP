using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class FixHandbook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "Handbooks");

            migrationBuilder.DropColumn(
                name: "LastEditedAt",
                table: "Handbooks");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Handbooks",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedUserId",
                table: "Handbooks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Handbooks");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserId",
                table: "Handbooks");

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "Handbooks",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditedAt",
                table: "Handbooks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("0a07b4b6-76b5-4758-ae87-d4ff24bb1d12"),
                column: "LastEditedAt",
                value: new DateTime(2019, 7, 17, 6, 35, 10, 989, DateTimeKind.Local).AddTicks(5454));

            migrationBuilder.UpdateData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("3e11f7c3-ee41-4bea-aaf1-1fda2d4cb001"),
                column: "LastEditedAt",
                value: new DateTime(2019, 7, 17, 6, 35, 10, 989, DateTimeKind.Local).AddTicks(5543));

            migrationBuilder.UpdateData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("5a1b9eac-d4a4-4d92-aa77-53c0fe1bead0"),
                column: "LastEditedAt",
                value: new DateTime(2019, 7, 17, 6, 35, 10, 989, DateTimeKind.Local).AddTicks(5477));

            migrationBuilder.UpdateData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("6a5587f0-f20e-47c5-9be9-de5aa3134c97"),
                column: "LastEditedAt",
                value: new DateTime(2019, 7, 17, 6, 35, 10, 989, DateTimeKind.Local).AddTicks(5440));

            migrationBuilder.UpdateData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("7a54980c-296e-4dee-b7cf-68a495c80ee0"),
                column: "LastEditedAt",
                value: new DateTime(2019, 7, 17, 6, 35, 10, 989, DateTimeKind.Local).AddTicks(5461));

            migrationBuilder.UpdateData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("90fdba24-d34f-4347-896e-3bc652328c1f"),
                column: "LastEditedAt",
                value: new DateTime(2019, 7, 17, 6, 35, 10, 989, DateTimeKind.Local).AddTicks(5537));

            migrationBuilder.UpdateData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("dac6d4fa-0502-43da-9368-9198e479f89d"),
                column: "LastEditedAt",
                value: new DateTime(2019, 7, 17, 6, 35, 10, 988, DateTimeKind.Local).AddTicks(3925));
        }
    }
}
