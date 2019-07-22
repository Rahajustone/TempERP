using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class TestData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: new Guid("63bcc86e-c1ad-4804-8881-9d8cc979e7a2"));

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: new Guid("9afda0d8-081b-43ff-ba01-9a0e1659c9cc"));

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("dac6d4fa-0502-43da-9368-9198e479f89d"), "Мужской" });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("0ce7a31f-dfd6-4bdc-ae57-32087c383705"), "Женский" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: new Guid("0ce7a31f-dfd6-4bdc-ae57-32087c383705"));

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: new Guid("dac6d4fa-0502-43da-9368-9198e479f89d"));

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("9afda0d8-081b-43ff-ba01-9a0e1659c9cc"), "Мужской" });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("63bcc86e-c1ad-4804-8881-9d8cc979e7a2"), "Женский" });
        }
    }
}
