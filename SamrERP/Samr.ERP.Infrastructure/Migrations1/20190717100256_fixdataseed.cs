using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class fixdataseed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("92ddaaaf-fd9f-4f99-8443-2bed011e9d78"),
                column: "ActionName",
                value: "FileCategory/All");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("92ddaaaf-fd9f-4f99-8443-2bed011e9d78"),
                column: "ActionName",
                value: "FileCategoryController/All");
        }
    }
}
