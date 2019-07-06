using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class FixUserLockReasons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserLockReason_UserLockReasonId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLockReason_AspNetUsers_CreatedUserId",
                table: "UserLockReason");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLockReason",
                table: "UserLockReason");

            migrationBuilder.RenameTable(
                name: "UserLockReason",
                newName: "UserLockReasons");

            migrationBuilder.RenameIndex(
                name: "IX_UserLockReason_CreatedUserId",
                table: "UserLockReasons",
                newName: "IX_UserLockReasons_CreatedUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLockReasons",
                table: "UserLockReasons",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserLockReasons_UserLockReasonId",
                table: "AspNetUsers",
                column: "UserLockReasonId",
                principalTable: "UserLockReasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLockReasons_AspNetUsers_CreatedUserId",
                table: "UserLockReasons",
                column: "CreatedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserLockReasons_UserLockReasonId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLockReasons_AspNetUsers_CreatedUserId",
                table: "UserLockReasons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLockReasons",
                table: "UserLockReasons");

            migrationBuilder.RenameTable(
                name: "UserLockReasons",
                newName: "UserLockReason");

            migrationBuilder.RenameIndex(
                name: "IX_UserLockReasons_CreatedUserId",
                table: "UserLockReason",
                newName: "IX_UserLockReason_CreatedUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLockReason",
                table: "UserLockReason",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserLockReason_UserLockReasonId",
                table: "AspNetUsers",
                column: "UserLockReasonId",
                principalTable: "UserLockReason",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLockReason_AspNetUsers_CreatedUserId",
                table: "UserLockReason",
                column: "CreatedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
