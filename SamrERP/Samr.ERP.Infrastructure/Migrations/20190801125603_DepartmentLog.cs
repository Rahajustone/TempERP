using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class DepartmentLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DepartmentLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    RootId = table.Column<Guid>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    DepartmentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentLogs_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentLogs_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: new Guid("9a3fcddb-4680-4206-b712-4e07df82e354"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 1, 15, 56, 3, 169, DateTimeKind.Local).AddTicks(6783));

            migrationBuilder.UpdateData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 1, 15, 56, 3, 168, DateTimeKind.Local).AddTicks(3823));

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentLogs_CreatedUserId",
                table: "DepartmentLogs",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentLogs_DepartmentId",
                table: "DepartmentLogs",
                column: "DepartmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentLogs");

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: new Guid("9a3fcddb-4680-4206-b712-4e07df82e354"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 1, 16, 13, 3, 609, DateTimeKind.Local).AddTicks(974));

            migrationBuilder.UpdateData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 1, 16, 13, 3, 606, DateTimeKind.Local).AddTicks(4127));
        }
    }
}
