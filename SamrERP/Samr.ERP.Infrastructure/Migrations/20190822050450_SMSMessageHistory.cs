using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class SMSMessageHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SMSMessageHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    ReceiverUserId = table.Column<Guid>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    SMPPSettingId = table.Column<Guid>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMSMessageHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SMSMessageHistories_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SMSMessageHistories_AspNetUsers_ReceiverUserId",
                        column: x => x.ReceiverUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SMSMessageHistories_SMPPSettings_SMPPSettingId",
                        column: x => x.SMPPSettingId,
                        principalTable: "SMPPSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SMSMessageHistories_CreatedUserId",
                table: "SMSMessageHistories",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SMSMessageHistories_ReceiverUserId",
                table: "SMSMessageHistories",
                column: "ReceiverUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SMSMessageHistories_SMPPSettingId",
                table: "SMSMessageHistories",
                column: "SMPPSettingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SMSMessageHistories");
        }
    }
}
