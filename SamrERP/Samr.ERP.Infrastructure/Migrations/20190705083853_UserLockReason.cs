using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class UserLockReason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserLockReasonId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserLockReason",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLockReason", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLockReason_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserLockReasonId",
                table: "AspNetUsers",
                column: "UserLockReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLockReason_CreatedUserId",
                table: "UserLockReason",
                column: "CreatedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserLockReason_UserLockReasonId",
                table: "AspNetUsers",
                column: "UserLockReasonId",
                principalTable: "UserLockReason",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserLockReason_UserLockReasonId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserLockReason");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserLockReasonId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserLockReasonId",
                table: "AspNetUsers");
        }
    }
}
