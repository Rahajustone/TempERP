using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class SeedDataToRussion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: new Guid("53b6c3b2-2223-4ee9-a8b9-edac5ca45e20"));

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: new Guid("afa798f7-6980-4187-9353-81f700ed79d9"));

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("800ebb18-f2a7-4869-bac7-c9a7161dcccd"), "Мужской" });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("07c41ef0-6cd2-48bc-a5fa-0fedc9748964"), "Женский" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: new Guid("07c41ef0-6cd2-48bc-a5fa-0fedc9748964"));

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: new Guid("800ebb18-f2a7-4869-bac7-c9a7161dcccd"));

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("afa798f7-6980-4187-9353-81f700ed79d9"), "Male" });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("53b6c3b2-2223-4ee9-a8b9-edac5ca45e20"), "Female" });
        }
    }
}
