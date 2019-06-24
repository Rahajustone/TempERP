using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class Employee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 32, nullable: false),
                    LastName = table.Column<string>(maxLength: 32, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 32, nullable: true),
                    PhotoPath = table.Column<string>(maxLength: 32, nullable: true),
                    GenderId = table.Column<int>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    HireDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 128, nullable: false),
                    LockDate = table.Column<DateTime>(nullable: false),
                    LockTypeId = table.Column<int>(nullable: false),
                    lockUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
