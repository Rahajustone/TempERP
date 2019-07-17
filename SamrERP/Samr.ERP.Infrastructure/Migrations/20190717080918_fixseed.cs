using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class fixseed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserFullName",
                table: "Handbooks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModifiedUserFullName",
                table: "Handbooks");
        }
    }
}
