using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class EmalSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    MailServerName = table.Column<string>(nullable: true),
                    MailServer = table.Column<string>(nullable: true),
                    MailPort = table.Column<int>(nullable: false),
                    SenderName = table.Column<string>(nullable: true),
                    Sender = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailSettings_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmailMessageHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    RecieverUserId = table.Column<Guid>(nullable: false),
                    RecieverEMail = table.Column<string>(nullable: true),
                    EmailSettingId = table.Column<Guid>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailMessageHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailMessageHistories_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmailMessageHistories_EmailSettings_EmailSettingId",
                        column: x => x.EmailSettingId,
                        principalTable: "EmailSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmailMessageHistories_AspNetUsers_RecieverUserId",
                        column: x => x.RecieverUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailMessageHistories_CreatedUserId",
                table: "EmailMessageHistories",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailMessageHistories_EmailSettingId",
                table: "EmailMessageHistories",
                column: "EmailSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailMessageHistories_RecieverUserId",
                table: "EmailMessageHistories",
                column: "RecieverUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailSettings_CreatedUserId",
                table: "EmailSettings",
                column: "CreatedUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailMessageHistories");

            migrationBuilder.DropTable(
                name: "EmailSettings");
        }
    }
}
