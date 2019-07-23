using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class filearchive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FileCategories",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "FileArchives",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    FileCategoryId = table.Column<Guid>(nullable: false),
                    FileName = table.Column<string>(maxLength: 1, nullable: false),
                    ShortDescription = table.Column<string>(maxLength: 128, nullable: false),
                    Description = table.Column<string>(maxLength: 512, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileArchives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileArchives_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FileArchives_FileCategories_FileCategoryId",
                        column: x => x.FileCategoryId,
                        principalTable: "FileCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileArchives_CreatedUserId",
                table: "FileArchives",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FileArchives_FileCategoryId",
                table: "FileArchives",
                column: "FileCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileArchives");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FileCategories",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64,
                oldNullable: true);
        }
    }
}
