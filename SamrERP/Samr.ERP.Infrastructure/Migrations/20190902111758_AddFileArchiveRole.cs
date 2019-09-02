using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class AddFileArchiveRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Category", "CategoryName", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("d8bc97fc-7a24-41d7-9612-b9e9ace30af9"), "FileArchive", "Файл Архиве", "8fa485ea-1ed7-40ba-8e01-4f7f9079f667", "Создание", "FileArchive.Create", "FILEARCHIVE.CREATE" },
                    { new Guid("be43f589-77ba-475d-bba8-af504aae540e"), "FileArchive", "Файл Архиве", "429f6fed-a00a-4cbb-a105-70f5c6b040aa", "Редактирование", "FileArchive.Edit", "FILEARCHIVE.EDIT" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("be43f589-77ba-475d-bba8-af504aae540e"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d8bc97fc-7a24-41d7-9612-b9e9ace30af9"));
        }
    }
}
