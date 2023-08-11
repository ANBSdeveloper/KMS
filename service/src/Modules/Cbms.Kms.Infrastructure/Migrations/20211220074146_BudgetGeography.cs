using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cbms.Kms.Infrastructure.Migrations
{
    public partial class BudgetGeography : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetDetails");

            migrationBuilder.DropSequence(
                name: "budget_detail_seq");

            migrationBuilder.CreateSequence(
                name: "budget_area_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "budget_branch_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "budget_zone_seq",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "BudgetAreas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    AreaId = table.Column<int>(nullable: false),
                    BudgetId = table.Column<int>(nullable: false),
                    AllocateAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    TempUsedAmount = table.Column<decimal>(nullable: false),
                    TempRemainAmount = table.Column<decimal>(nullable: false),
                    UsedAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    RemainAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetAreas_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetAreas_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetAreas_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetAreas_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BudgetBranches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    BranchId = table.Column<int>(nullable: false),
                    BudgetId = table.Column<int>(nullable: false),
                    AllocateAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    TempUsedAmount = table.Column<decimal>(nullable: false),
                    TempRemainAmount = table.Column<decimal>(nullable: false),
                    UsedAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    RemainAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetBranches_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetBranches_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetBranches_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetBranches_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BudgetZones",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    ZoneId = table.Column<int>(nullable: false),
                    BudgetId = table.Column<int>(nullable: false),
                    AllocateAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    TempUsedAmount = table.Column<decimal>(nullable: false),
                    TempRemainAmount = table.Column<decimal>(nullable: false),
                    UsedAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    RemainAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetZones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetZones_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetZones_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetZones_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetZones_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetAreas_AreaId",
                table: "BudgetAreas",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetAreas_BudgetId",
                table: "BudgetAreas",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetAreas_CreatorUserId",
                table: "BudgetAreas",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetAreas_LastModifierUserId",
                table: "BudgetAreas",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBranches_BranchId",
                table: "BudgetBranches",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBranches_BudgetId",
                table: "BudgetBranches",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBranches_CreatorUserId",
                table: "BudgetBranches",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetBranches_LastModifierUserId",
                table: "BudgetBranches",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetZones_BudgetId",
                table: "BudgetZones",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetZones_CreatorUserId",
                table: "BudgetZones",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetZones_LastModifierUserId",
                table: "BudgetZones",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetZones_ZoneId",
                table: "BudgetZones",
                column: "ZoneId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetAreas");

            migrationBuilder.DropTable(
                name: "BudgetBranches");

            migrationBuilder.DropTable(
                name: "BudgetZones");

            migrationBuilder.DropSequence(
                name: "budget_area_seq");

            migrationBuilder.DropSequence(
                name: "budget_branch_seq");

            migrationBuilder.DropSequence(
                name: "budget_zone_seq");

            migrationBuilder.CreateSequence(
                name: "budget_detail_seq",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "BudgetDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AllocateAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    BudgetId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true),
                    RemainAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    TempRemainAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TempUsedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UsedAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetDetails_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetDetails_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetDetails_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetDetails_Staffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_BudgetId",
                table: "BudgetDetails",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_CreatorUserId",
                table: "BudgetDetails",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_LastModifierUserId",
                table: "BudgetDetails",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_StaffId",
                table: "BudgetDetails",
                column: "StaffId");
        }
    }
}
