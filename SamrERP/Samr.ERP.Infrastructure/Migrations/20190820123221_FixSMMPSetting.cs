using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class FixSMMPSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SMPPSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    Host = table.Column<string>(nullable: true),
                    PortNumber = table.Column<int>(nullable: false),
                    SystemId = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMPPSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SMPPSettings_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SMPPSettings_CreatedUserId",
                table: "SMPPSettings",
                column: "CreatedUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SMPPSettings");
        }
    }
}
