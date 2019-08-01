using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Category", "CategoryName", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("2702bcdd-104b-475d-14b5-08d70c357974"), "Employee", "Сотрудники", "23F4A768-BCF3-4BA4-8D20-CC3E4A9C333A", "Просмотр списка", "Employee.All", "EMPLOYEE.ALL" },
                    { new Guid("29ecf6ce-b82f-4fc5-ae01-08d70cf9f908"), "Employee", "Сотрудники", "B8EFD7E4-72E8-4110-96CC-A531AD35D9B4", "Создание", "Employee.Create", "EMPLOYEE.CREATE" },
                    { new Guid("a8eb0e97-eaaa-4976-ae02-08d70cf9f908"), "Employee", "Сотрудники", "98003B79-EE18-4D7F-B8A5-357E74E8F77A", "Редактирование", "Employee.Edit", "EMPLOYEE.EDIT" },
                    { new Guid("c5dbeaab-86a3-4400-b50a-08d70e6b40dc"), "Employee", "Сотрудники", "36271C6B-8972-4A69-90D0-D9921B6F90D3", "Подробная информация", "Employee.Details", "EMPLOYEE.DETAILS" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ChangePasswordConfirmationCode", "ChangePasswordConfirmationCodeExpires", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockDate", "LockUserId", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserLockReasonId", "UserName" },
                values: new object[] { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), 0, null, null, "9132A248-C5F8-4B01-91FB-9AF3777FCA48", "migdev@mig.tj", false, null, null, false, null, null, "000000000", "AQAAAAEAACcQAAAAEOvH5DQ4ttSuk1j8EVrg4uyxzHJGcnZbuhkdRvuppk2ttPByA/FjKpVcrA001HW68w==", "000000000", false, "83147D9F-26BC-486F-AE7E-5DD581362FAA", false, null, "000000000" });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("dac6d4fa-0502-43da-9368-9198e479f89d"), "Мужской" },
                    { new Guid("0ce7a31f-dfd6-4bdc-ae57-32087c383705"), "Женский" }
                });

            migrationBuilder.InsertData(
                table: "EmailSettings",
                columns: new[] { "Id", "CreatedAt", "CreatedUserId", "EnabledSSL", "IsActive", "IsDefault", "MailPort", "MailServer", "MailServerName", "Password", "Sender", "SenderName" },
                values: new object[] { new Guid("9a3fcddb-4680-4206-b712-4e07df82e354"), new DateTime(2019, 8, 1, 16, 3, 2, 959, DateTimeKind.Local).AddTicks(6755), new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), false, true, true, 465, "smtp.yandex.ru", "Yandex Mail", "formignow", "migdev@mig.tj", "Mig Dev" });

            migrationBuilder.InsertData(
                table: "UserLockReasons",
                columns: new[] { "Id", "CreatedAt", "CreatedUserId", "IsActive", "Name" },
                values: new object[] { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new DateTime(2019, 8, 1, 16, 3, 2, 957, DateTimeKind.Local).AddTicks(5738), new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), false, null });
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

            migrationBuilder.DeleteData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: new Guid("9a3fcddb-4680-4206-b712-4e07df82e354"));

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: new Guid("0ce7a31f-dfd6-4bdc-ae57-32087c383705"));

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: new Guid("dac6d4fa-0502-43da-9368-9198e479f89d"));

            migrationBuilder.DeleteData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"));
        }
    }
}
