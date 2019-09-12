using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Samr.ERP.Infrastructure.Migrations
{
    public partial class TableLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeLockReasonLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLockReasonLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeLockReasonLogs_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FileCategoryLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    FileCategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileCategoryLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileCategoryLogs_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FileCategoryLogs_FileCategories_FileCategoryId",
                        column: x => x.FileCategoryId,
                        principalTable: "FileCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NationalityLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    NationalityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NationalityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NationalityLogs_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NationalityLogs_Nationalities_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "Nationalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NewsCategoryLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    NewsCategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsCategoryLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsCategoryLogs_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NewsCategoryLogs_NewsCategories_NewsCategoryId",
                        column: x => x.NewsCategoryId,
                        principalTable: "NewsCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PositionLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    DepartmentId = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    PositionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PositionLogs_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PositionLogs_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PositionLogs_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsefulLinkCategoryLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UsefulLinkCategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsefulLinkCategoryLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsefulLinkCategoryLogs_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsefulLinkCategoryLogs_UsefulLinkCategories_UsefulLinkCateg~",
                        column: x => x.UsefulLinkCategoryId,
                        principalTable: "UsefulLinkCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: new Guid("9a3fcddb-4680-4206-b712-4e07df82e354"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 3, 12, 21, 20, 228, DateTimeKind.Local).AddTicks(2473));

            migrationBuilder.UpdateData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 3, 12, 21, 20, 226, DateTimeKind.Local).AddTicks(9928));

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLockReasonLogs_CreatedUserId",
                table: "EmployeeLockReasonLogs",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FileCategoryLogs_CreatedUserId",
                table: "FileCategoryLogs",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FileCategoryLogs_FileCategoryId",
                table: "FileCategoryLogs",
                column: "FileCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_NationalityLogs_CreatedUserId",
                table: "NationalityLogs",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NationalityLogs_NationalityId",
                table: "NationalityLogs",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsCategoryLogs_CreatedUserId",
                table: "NewsCategoryLogs",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsCategoryLogs_NewsCategoryId",
                table: "NewsCategoryLogs",
                column: "NewsCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionLogs_CreatedUserId",
                table: "PositionLogs",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionLogs_DepartmentId",
                table: "PositionLogs",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionLogs_PositionId",
                table: "PositionLogs",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_UsefulLinkCategoryLogs_CreatedUserId",
                table: "UsefulLinkCategoryLogs",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsefulLinkCategoryLogs_UsefulLinkCategoryId",
                table: "UsefulLinkCategoryLogs",
                column: "UsefulLinkCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeLockReasonLogs");

            migrationBuilder.DropTable(
                name: "FileCategoryLogs");

            migrationBuilder.DropTable(
                name: "NationalityLogs");

            migrationBuilder.DropTable(
                name: "NewsCategoryLogs");

            migrationBuilder.DropTable(
                name: "PositionLogs");

            migrationBuilder.DropTable(
                name: "UsefulLinkCategoryLogs");

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: new Guid("9a3fcddb-4680-4206-b712-4e07df82e354"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 2, 12, 59, 0, 273, DateTimeKind.Local).AddTicks(7659));

            migrationBuilder.UpdateData(
                table: "UserLockReasons",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2019, 8, 2, 12, 59, 0, 272, DateTimeKind.Local).AddTicks(6310));
        }
    }
}
