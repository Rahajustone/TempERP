using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class AddRolesNewsAndUsefulLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Category", "CategoryName", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("dcd9b7ea-d8be-41e6-8da1-468e0e9b9f95"), "UsefulLink", "Полезные ссылки", "bcda3231-c9c9-4afb-a730-879d4099a712", "Создание", "UsefulLink.Create", "USEFULLINK.CREATE" },
                    { new Guid("ce6ab40d-23a0-4b9a-8ad2-65f4d38a9a76"), "UsefulLink", "Полезные ссылки", "6d356f9a-2318-4434-afd4-c4bea23aa7d0", "Редактирование", "UsefulLink.Edit", "USEFULLINK.EDIT" },
                    { new Guid("2607ef6c-1f5b-49c7-8839-c2dec3471dc5"), "News", "новостей", "4dee761f-8d23-4d92-bb37-32539bc7f353", "Создание", "News.Create", "NEWS.CREATE" },
                    { new Guid("7d5ecac9-b964-438d-8911-383569d4d804"), "News", "новостей", "2c00e616-9534-4c49-8255-bfed5e0db7b2", "Редактирование", "News.Edit", "NEWS.EDIT" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2607ef6c-1f5b-49c7-8839-c2dec3471dc5"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7d5ecac9-b964-438d-8911-383569d4d804"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ce6ab40d-23a0-4b9a-8ad2-65f4d38a9a76"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("dcd9b7ea-d8be-41e6-8da1-468e0e9b9f95"));
        }
    }
}
