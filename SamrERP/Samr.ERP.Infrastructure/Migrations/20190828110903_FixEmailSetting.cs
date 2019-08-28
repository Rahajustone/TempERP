using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class FixEmailSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailMessageHistories_AspNetUsers_RecieverUserId",
                table: "EmailMessageHistories");

            migrationBuilder.RenameColumn(
                name: "RecieverUserId",
                table: "EmailMessageHistories",
                newName: "ReceiverUserId");

            migrationBuilder.RenameColumn(
                name: "RecieverEMail",
                table: "EmailMessageHistories",
                newName: "ReceiverEmail");

            migrationBuilder.RenameIndex(
                name: "IX_EmailMessageHistories_RecieverUserId",
                table: "EmailMessageHistories",
                newName: "IX_EmailMessageHistories_ReceiverUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailMessageHistories_AspNetUsers_ReceiverUserId",
                table: "EmailMessageHistories",
                column: "ReceiverUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailMessageHistories_AspNetUsers_ReceiverUserId",
                table: "EmailMessageHistories");

            migrationBuilder.RenameColumn(
                name: "ReceiverUserId",
                table: "EmailMessageHistories",
                newName: "RecieverUserId");

            migrationBuilder.RenameColumn(
                name: "ReceiverEmail",
                table: "EmailMessageHistories",
                newName: "RecieverEMail");

            migrationBuilder.RenameIndex(
                name: "IX_EmailMessageHistories_ReceiverUserId",
                table: "EmailMessageHistories",
                newName: "IX_EmailMessageHistories_RecieverUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailMessageHistories_AspNetUsers_RecieverUserId",
                table: "EmailMessageHistories",
                column: "RecieverUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
