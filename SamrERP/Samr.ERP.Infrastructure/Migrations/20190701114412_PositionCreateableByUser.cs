using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class PositionCreateableByUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedUserId",
                table: "Positions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Positions_CreatedUserId",
                table: "Positions",
                column: "CreatedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_AspNetUsers_CreatedUserId",
                table: "Positions",
                column: "CreatedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Positions_AspNetUsers_CreatedUserId",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Positions_CreatedUserId",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Positions");
        }
    }
}
