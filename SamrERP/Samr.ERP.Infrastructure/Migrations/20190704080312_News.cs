using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class News : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    CatId = table.Column<Guid>(nullable: false),
                    NewsCategoryId = table.Column<Guid>(nullable: true),
                    Title = table.Column<string>(maxLength: 64, nullable: false),
                    ShortDescription = table.Column<string>(maxLength: 256, nullable: false),
                    Description = table.Column<string>(nullable: false),
                    PublishAt = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                    table.ForeignKey(
                        name: "FK_News_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_News_NewsCategories_NewsCategoryId",
                        column: x => x.NewsCategoryId,
                        principalTable: "NewsCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_News_CreatedUserId",
                table: "News",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_News_NewsCategoryId",
                table: "News",
                column: "NewsCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");
        }
    }
}
