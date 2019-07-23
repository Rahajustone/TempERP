using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class SeedFileCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Handbooks",
                columns: new[] { "Id", "ActionName", "DisplayName", "LastModifiedAt", "LastModifiedUserFullName", "LastModifiedUserId", "Name" },
                values: new object[] { new Guid("92ddaaaf-fd9f-4f99-8443-2bed011e9d78"), "FileCategoryController/All", "Категория файла", null, null, null, "FileCategory" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("92ddaaaf-fd9f-4f99-8443-2bed011e9d78"));
        }
    }
}
