using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class EmployeeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 32, nullable: false),
                    LastName = table.Column<string>(maxLength: 32, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 32, nullable: true),
                    PositionId = table.Column<Guid>(nullable: false),
                    ImageName = table.Column<string>(maxLength: 32, nullable: true),
                    GenderId = table.Column<Guid>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Phone = table.Column<string>(maxLength: 9, nullable: false),
                    HireDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 128, nullable: false),
                    FactualAddress = table.Column<string>(maxLength: 256, nullable: true),
                    LockDate = table.Column<DateTime>(nullable: true),
                    EmployeeLockReasonId = table.Column<Guid>(nullable: true),
                    LockUserId = table.Column<Guid>(nullable: true),
                    PassportNumber = table.Column<string>(maxLength: 32, nullable: true),
                    PassportIssuer = table.Column<string>(maxLength: 64, nullable: true),
                    PassportIssueDate = table.Column<DateTime>(nullable: true),
                    NationalityId = table.Column<Guid>(nullable: true),
                    PassportAddress = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true),
                    PhotoPath = table.Column<string>(nullable: true),
                    PassportScanPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_EmployeeLockReasons_EmployeeLockReasonId",
                        column: x => x.EmployeeLockReasonId,
                        principalTable: "EmployeeLockReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_LockUserId",
                        column: x => x.LockUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Nationalities_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "Nationalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: new Guid("9a3fcddb-4680-4206-b712-4e07df82e354"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 1, 16, 13, 3, 609, DateTimeKind.Local).AddTicks(974));

            migrationBuilder.UpdateData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 1, 16, 13, 3, 606, DateTimeKind.Local).AddTicks(4127));

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CreatedUserId",
                table: "Employees",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeLockReasonId",
                table: "Employees",
                column: "EmployeeLockReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_GenderId",
                table: "Employees",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LockUserId",
                table: "Employees",
                column: "LockUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_NationalityId",
                table: "Employees",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PositionId",
                table: "Employees",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: new Guid("9a3fcddb-4680-4206-b712-4e07df82e354"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 1, 16, 3, 2, 959, DateTimeKind.Local).AddTicks(6755));

            migrationBuilder.UpdateData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 1, 16, 3, 2, 957, DateTimeKind.Local).AddTicks(5738));
        }
    }
}
