using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class ChangeEntityName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileArchives_FileCategories_FileCategoryId",
                table: "FileArchives");

            migrationBuilder.DropTable(
                name: "FileCategoryLogs");

            migrationBuilder.DropTable(
                name: "FileCategories");

            migrationBuilder.CreateTable(
                name: "FileArchiveCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileArchiveCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileArchiveCategories_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FileArchiveCategoryLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    FileCategoryId = table.Column<Guid>(nullable: false),
                    FileArchiveCategoryId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileArchiveCategoryLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileArchiveCategoryLogs_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FileArchiveCategoryLogs_FileArchiveCategories_FileArchiveCa~",
                        column: x => x.FileArchiveCategoryId,
                        principalTable: "FileArchiveCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: new Guid("9a3fcddb-4680-4206-b712-4e07df82e354"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 8, 14, 30, 29, 207, DateTimeKind.Local).AddTicks(7254));

            migrationBuilder.UpdateData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 8, 14, 30, 29, 206, DateTimeKind.Local).AddTicks(5478));

            migrationBuilder.CreateIndex(
                name: "IX_FileArchiveCategories_CreatedUserId",
                table: "FileArchiveCategories",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FileArchiveCategoryLogs_CreatedUserId",
                table: "FileArchiveCategoryLogs",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FileArchiveCategoryLogs_FileArchiveCategoryId",
                table: "FileArchiveCategoryLogs",
                column: "FileArchiveCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileArchives_FileArchiveCategories_FileCategoryId",
                table: "FileArchives",
                column: "FileCategoryId",
                principalTable: "FileArchiveCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileArchives_FileArchiveCategories_FileCategoryId",
                table: "FileArchives");

            migrationBuilder.DropTable(
                name: "FileArchiveCategoryLogs");

            migrationBuilder.DropTable(
                name: "FileArchiveCategories");

            migrationBuilder.CreateTable(
                name: "FileCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileCategories_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FileCategoryLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    FileCategoryId = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileCategoryLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileCategoryLogs_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FileCategoryLogs_FileCategories_FileCategoryId",
                        column: x => x.FileCategoryId,
                        principalTable: "FileCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: new Guid("9a3fcddb-4680-4206-b712-4e07df82e354"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 8, 11, 0, 16, 68, DateTimeKind.Local).AddTicks(7579));

            migrationBuilder.UpdateData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 8, 11, 0, 16, 67, DateTimeKind.Local).AddTicks(2435));

            migrationBuilder.CreateIndex(
                name: "IX_FileCategories_CreatedUserId",
                table: "FileCategories",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FileCategoryLogs_CreatedUserId",
                table: "FileCategoryLogs",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FileCategoryLogs_FileCategoryId",
                table: "FileCategoryLogs",
                column: "FileCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileArchives_FileCategories_FileCategoryId",
                table: "FileArchives",
                column: "FileCategoryId",
                principalTable: "FileCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
