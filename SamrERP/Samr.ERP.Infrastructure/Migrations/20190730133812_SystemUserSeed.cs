using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class SystemUserSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "LockUserId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ChangePasswordConfirmationCode", "ChangePasswordConfirmationCodeExpires", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockDate", "LockUserId", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserLockReasonId", "UserName" },
                values: new object[] { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), 0, null, null, "9132A248-C5F8-4B01-91FB-9AF3777FCA48", "migdev@mig.tj", false, null, null, false, null, null, "000000000", "AQAAAAEAACcQAAAAEOvH5DQ4ttSuk1j8EVrg4uyxzHJGcnZbuhkdRvuppk2ttPByA/FjKpVcrA001HW68w==", null, false, null, false, null, "000000000" });

            migrationBuilder.InsertData(
                table: "UserLockReasons",
                columns: new[] { "Id", "CreatedAt", "CreatedUserId", "IsActive", "Name" },
                values: new object[] { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), false, null });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LockUserId",
                table: "AspNetUsers",
                column: "LockUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_LockUserId",
                table: "AspNetUsers",
                column: "LockUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_LockUserId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LockUserId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"));

            migrationBuilder.DropColumn(
                name: "LockUserId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }
    }
}
