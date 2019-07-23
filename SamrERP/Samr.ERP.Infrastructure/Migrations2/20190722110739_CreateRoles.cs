using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class CreateRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Category", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("2702bcdd-104b-475d-14b5-08d70c357974"), "Employee", "23F4A768-BCF3-4BA4-8D20-CC3E4A9C333A", "Просмотр списка", "Employee.All", "EMPLOYEE.ALL" },
                    { new Guid("29ecf6ce-b82f-4fc5-ae01-08d70cf9f908"), "Employee", "B8EFD7E4-72E8-4110-96CC-A531AD35D9B4", "Создание", "Employee.Create", "EMPLOYEE.CREATE" },
                    { new Guid("a8eb0e97-eaaa-4976-ae02-08d70cf9f908"), "Employee", "98003B79-EE18-4D7F-B8A5-357E74E8F77A", "Редактирование", "Employee.Edit", "EMPLOYEE.EDIT" },
                    { new Guid("c5dbeaab-86a3-4400-b50a-08d70e6b40dc"), "Employee", "36271C6B-8972-4A69-90D0-D9921B6F90D3", "Подробная информация", "Employee.Details", "EMPLOYEE.DETAILS" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2702bcdd-104b-475d-14b5-08d70c357974"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("29ecf6ce-b82f-4fc5-ae01-08d70cf9f908"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a8eb0e97-eaaa-4976-ae02-08d70cf9f908"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c5dbeaab-86a3-4400-b50a-08d70e6b40dc"));
        }
    }
}
