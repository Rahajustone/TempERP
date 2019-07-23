using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class Handbook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Handbooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ActionName = table.Column<string>(nullable: false),
                    CreatedUserName = table.Column<string>(nullable: true),
                    LastEditedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Handbooks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Handbooks",
                columns: new[] { "Id", "ActionName", "CreatedUserName", "LastEditedAt", "Name" },
                values: new object[] { new Guid("dac6d4fa-0502-43da-9368-9198e479f89d"), "Nationality/All", null, new DateTime(2019, 7, 16, 8, 44, 53, 782, DateTimeKind.Local).AddTicks(6556), "Nationality" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Handbooks");
        }
    }
}
