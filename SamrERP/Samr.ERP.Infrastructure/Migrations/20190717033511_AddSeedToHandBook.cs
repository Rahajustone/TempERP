using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class AddSeedToHandBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("dac6d4fa-0502-43da-9368-9198e479f89d"),
                columns: new[] { "DisplayName", "LastEditedAt" },
                values: new object[] { "Национальность", new DateTime(2019, 7, 17, 6, 35, 10, 988, DateTimeKind.Local).AddTicks(3925) });

            migrationBuilder.InsertData(
                table: "Handbooks",
                columns: new[] { "Id", "ActionName", "CreatedUserName", "DisplayName", "LastEditedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("6a5587f0-f20e-47c5-9be9-de5aa3134c97"), "Department/All", null, "Отдел", new DateTime(2019, 7, 17, 6, 35, 10, 989, DateTimeKind.Local).AddTicks(5440), "Department" },
                    { new Guid("0a07b4b6-76b5-4758-ae87-d4ff24bb1d12"), "NewsCategories/All", null, "Категория полезных ссылок", new DateTime(2019, 7, 17, 6, 35, 10, 989, DateTimeKind.Local).AddTicks(5454), "NewsCategories" },
                    { new Guid("7a54980c-296e-4dee-b7cf-68a495c80ee0"), "EmployeeLockReason/All", null, "Причина блокировки сотрудника", new DateTime(2019, 7, 17, 6, 35, 10, 989, DateTimeKind.Local).AddTicks(5461), "EmployeeLockReason" },
                    { new Guid("5a1b9eac-d4a4-4d92-aa77-53c0fe1bead0"), "Position/All", null, "Позиция", new DateTime(2019, 7, 17, 6, 35, 10, 989, DateTimeKind.Local).AddTicks(5477), "Position" },
                    { new Guid("90fdba24-d34f-4347-896e-3bc652328c1f"), "UserLockReason/All", null, "Причина блокировки пользователя", new DateTime(2019, 7, 17, 6, 35, 10, 989, DateTimeKind.Local).AddTicks(5537), "UserLockReason" },
                    { new Guid("3e11f7c3-ee41-4bea-aaf1-1fda2d4cb001"), "UsefulLinkCategory/All", null, "Полезная ссылка", new DateTime(2019, 7, 17, 6, 35, 10, 989, DateTimeKind.Local).AddTicks(5543), "UsefulLinkCategory" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("0a07b4b6-76b5-4758-ae87-d4ff24bb1d12"));

            migrationBuilder.DeleteData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("3e11f7c3-ee41-4bea-aaf1-1fda2d4cb001"));

            migrationBuilder.DeleteData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("5a1b9eac-d4a4-4d92-aa77-53c0fe1bead0"));

            migrationBuilder.DeleteData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("6a5587f0-f20e-47c5-9be9-de5aa3134c97"));

            migrationBuilder.DeleteData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("7a54980c-296e-4dee-b7cf-68a495c80ee0"));

            migrationBuilder.DeleteData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("90fdba24-d34f-4347-896e-3bc652328c1f"));

            migrationBuilder.UpdateData(
                table: "Handbooks",
                keyColumn: "Id",
                keyValue: new Guid("dac6d4fa-0502-43da-9368-9198e479f89d"),
                columns: new[] { "DisplayName", "LastEditedAt" },
                values: new object[] { "Test", new DateTime(2019, 7, 16, 14, 48, 12, 571, DateTimeKind.Local).AddTicks(3114) });
        }
    }
}
