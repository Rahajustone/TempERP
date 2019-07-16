using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class AddFieldToHandbook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "Handbooks",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("dac6d4fa-0502-43da-9368-9198e479f89d"),
                columns: new[] { "DisplayName", "LastEditedAt" },
                values: new object[] { "Test", new DateTime(2019, 7, 16, 11, 56, 8, 778, DateTimeKind.Local).AddTicks(4488) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Handbooks");

            migrationBuilder.UpdateData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("dac6d4fa-0502-43da-9368-9198e479f89d"),
                column: "LastEditedAt",
                value: new DateTime(2019, 7, 16, 8, 44, 53, 782, DateTimeKind.Local).AddTicks(6556));
        }
    }
}
