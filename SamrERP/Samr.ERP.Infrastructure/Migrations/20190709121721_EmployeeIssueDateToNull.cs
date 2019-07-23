using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class EmployeeIssueDateToNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "Genders",
            //    keyColumn: "Id",
            //    keyValue: new Guid("07c41ef0-6cd2-48bc-a5fa-0fedc9748964"));

            //migrationBuilder.DeleteData(
            //    table: "Genders",
            //    keyColumn: "Id",
            //    keyValue: new Guid("800ebb18-f2a7-4869-bac7-c9a7161dcccd"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PassportIssueDate",
                table: "Employees",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("9afda0d8-081b-43ff-ba01-9a0e1659c9cc"), "Мужской" });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("63bcc86e-c1ad-4804-8881-9d8cc979e7a2"), "Женский" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        //    migrationBuilder.DeleteData(
        //        table: "Genders",
        //        keyColumn: "Id",
        //        keyValue: new Guid("63bcc86e-c1ad-4804-8881-9d8cc979e7a2"));

        //    migrationBuilder.DeleteData(
        //        table: "Genders",
        //        keyColumn: "Id",
        //        keyValue: new Guid("9afda0d8-081b-43ff-ba01-9a0e1659c9cc"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PassportIssueDate",
                table: "Employees",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("800ebb18-f2a7-4869-bac7-c9a7161dcccd"), "Мужской" });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("07c41ef0-6cd2-48bc-a5fa-0fedc9748964"), "Женский" });
        }
    }
}
