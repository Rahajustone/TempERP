using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class UserChangePasswodConfirmaionCod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChangePasswordConfirmationCode",
                table: "AspNetUsers",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangePasswordConfirmationCodeExpires",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangePasswordConfirmationCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ChangePasswordConfirmationCodeExpires",
                table: "AspNetUsers");
        }
    }
}
