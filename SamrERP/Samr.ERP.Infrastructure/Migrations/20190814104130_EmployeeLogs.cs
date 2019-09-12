using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class EmployeeLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeLogs",
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
                    PassportScanPath = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeLogs_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeLogs_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeLogs_EmployeeLockReasons_EmployeeLockReasonId",
                        column: x => x.EmployeeLockReasonId,
                        principalTable: "EmployeeLockReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeLogs_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeLogs_AspNetUsers_LockUserId",
                        column: x => x.LockUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeLogs_Nationalities_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "Nationalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeLogs_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeLogs_AspNetUsers_UserId",
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
                value: new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLogs_CreatedUserId",
                table: "EmployeeLogs",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLogs_EmployeeId",
                table: "EmployeeLogs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLogs_EmployeeLockReasonId",
                table: "EmployeeLogs",
                column: "EmployeeLockReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLogs_GenderId",
                table: "EmployeeLogs",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLogs_LockUserId",
                table: "EmployeeLogs",
                column: "LockUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLogs_NationalityId",
                table: "EmployeeLogs",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLogs_PositionId",
                table: "EmployeeLogs",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLogs_UserId",
                table: "EmployeeLogs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeLogs");

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: new Guid("9a3fcddb-4680-4206-b712-4e07df82e354"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 13, 17, 32, 0, 362, DateTimeKind.Local).AddTicks(7217));

            migrationBuilder.UpdateData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 13, 17, 32, 0, 361, DateTimeKind.Local).AddTicks(4487));
        }
    }
}
