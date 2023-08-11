using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cbms.Kms.Infrastructure.Migrations
{
    public partial class PosmInvestment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "PosmItems");

            migrationBuilder.DropColumn(
                name: "Qty",
                table: "PosmItems");

            migrationBuilder.DropColumn(
                name: "SideWidth1",
                table: "PosmItems");

            migrationBuilder.DropColumn(
                name: "SideWidth2",
                table: "PosmItems");

            migrationBuilder.DropColumn(
                name: "UnitValue",
                table: "PosmItems");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "PosmItems");

            migrationBuilder.CreateSequence(
                name: "posm_investment_item_history_seq");

            migrationBuilder.CreateSequence(
                name: "posm_investment_item_seq",
                incrementBy: 5);

            migrationBuilder.CreateSequence(
                name: "posm_investment_seq");

            migrationBuilder.CreateSequence(
                name: "posm_sales_commitment_seq",
                incrementBy: 5);

            migrationBuilder.CreateTable(
                name: "PosmInvestments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    RegisterStaffId = table.Column<int>(nullable: false),
                    BudgetId = table.Column<int>(nullable: false),
                    CustomerLocationId = table.Column<int>(nullable: false),
                    CurrentSalesAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    ShopPanelPhoto1 = table.Column<string>(unicode: false, nullable: true),
                    ShopPanelPhoto2 = table.Column<string>(unicode: false, nullable: true),
                    ShopPanelPhoto3 = table.Column<string>(unicode: false, nullable: true),
                    ShopPanelPhoto4 = table.Column<string>(unicode: false, nullable: true),
                    VisibilityPhoto1 = table.Column<string>(unicode: false, nullable: true),
                    VisibilityPhoto2 = table.Column<string>(unicode: false, nullable: true),
                    VisibilityPhoto3 = table.Column<string>(unicode: false, nullable: true),
                    VisibilityPhoto4 = table.Column<string>(unicode: false, nullable: true),
                    VisibilityCompetitorPhoto1 = table.Column<string>(unicode: false, nullable: true),
                    VisibilityCompetitorPhoto2 = table.Column<string>(unicode: false, nullable: true),
                    VisibilityCompetitorPhoto3 = table.Column<string>(unicode: false, nullable: true),
                    VisibilityCompetitorPhoto4 = table.Column<string>(unicode: false, nullable: true),
                    Note = table.Column<string>(nullable: true),
                    SetupContact1 = table.Column<string>(nullable: true),
                    SetupContact2 = table.Column<string>(nullable: true),
                    InvestmentAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    CommitmentAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    CycleId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosmInvestments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PosmInvestments_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmInvestments_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmInvestments_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmInvestments_CustomerLocations_CustomerLocationId",
                        column: x => x.CustomerLocationId,
                        principalTable: "CustomerLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmInvestments_Cycles_CycleId",
                        column: x => x.CycleId,
                        principalTable: "Cycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmInvestments_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmInvestments_Staffs_RegisterStaffId",
                        column: x => x.RegisterStaffId,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PosmInvestmentItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    PosmClassId = table.Column<int>(nullable: false),
                    PosmItemId = table.Column<int>(nullable: false),
                    PanelShopName = table.Column<string>(maxLength: 500, nullable: true),
                    PanelShopPhone = table.Column<string>(maxLength: 50, nullable: true),
                    PanelShopAddress = table.Column<string>(maxLength: 1000, nullable: true),
                    PanelOtherInfo = table.Column<string>(maxLength: 1000, nullable: true),
                    PanelProductLine = table.Column<string>(maxLength: 1000, nullable: true),
                    PosmCatalogId = table.Column<int>(nullable: false),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SideWidth1 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SideWidth2 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Qty = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalActualCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Size = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PosmValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SetupPlanDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    Status = table.Column<int>(nullable: false),
                    RequestType = table.Column<int>(nullable: false),
                    RequestReason = table.Column<string>(maxLength: 1000, nullable: true),
                    Photo1 = table.Column<string>(unicode: false, nullable: true),
                    Photo2 = table.Column<string>(unicode: false, nullable: true),
                    Photo3 = table.Column<string>(unicode: false, nullable: true),
                    Photo4 = table.Column<string>(unicode: false, nullable: true),
                    PosmInvestmentId = table.Column<int>(nullable: false),
                    OperationPhoto1 = table.Column<string>(unicode: false, nullable: true),
                    OperationPhoto2 = table.Column<string>(unicode: false, nullable: true),
                    OperationPhoto3 = table.Column<string>(unicode: false, nullable: true),
                    OperationPhoto4 = table.Column<string>(unicode: false, nullable: true),
                    OperationLink = table.Column<string>(unicode: false, nullable: true),
                    OperationNote = table.Column<string>(maxLength: 1000, nullable: true),
                    OperationDate = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    AcceptancePhoto1 = table.Column<string>(unicode: false, nullable: true),
                    AcceptancePhoto2 = table.Column<string>(unicode: false, nullable: true),
                    AcceptancePhoto3 = table.Column<string>(unicode: false, nullable: true),
                    AcceptancePhoto4 = table.Column<string>(unicode: false, nullable: true),
                    AcceptanceDate = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    AcceptanceNote = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosmInvestmentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PosmInvestmentItems_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmInvestmentItems_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmInvestmentItems_PosmCatalogs_PosmCatalogId",
                        column: x => x.PosmCatalogId,
                        principalTable: "PosmCatalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmInvestmentItems_PosmClasses_PosmClassId",
                        column: x => x.PosmClassId,
                        principalTable: "PosmClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmInvestmentItems_PosmInvestments_PosmInvestmentId",
                        column: x => x.PosmInvestmentId,
                        principalTable: "PosmInvestments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PosmInvestmentItems_PosmItems_PosmItemId",
                        column: x => x.PosmItemId,
                        principalTable: "PosmItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PosmSalesCommitments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    PosmInvestmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosmSalesCommitments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PosmSalesCommitments_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmSalesCommitments_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmSalesCommitments_PosmInvestments_PosmInvestmentId",
                        column: x => x.PosmInvestmentId,
                        principalTable: "PosmInvestments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PosmInvestmentItemHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    PostInvestmentItemId = table.Column<int>(nullable: false),
                    PosmInvestmentItemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosmInvestmentItemHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PosmInvestmentItemHistories_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmInvestmentItemHistories_PosmInvestmentItems_PosmInvestmentItemId",
                        column: x => x.PosmInvestmentItemId,
                        principalTable: "PosmInvestmentItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PosmInvestmentItemHistories_CreatorUserId",
                table: "PosmInvestmentItemHistories",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmInvestmentItemHistories_PosmInvestmentItemId",
                table: "PosmInvestmentItemHistories",
                column: "PosmInvestmentItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmInvestmentItems_CreatorUserId",
                table: "PosmInvestmentItems",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmInvestmentItems_LastModifierUserId",
                table: "PosmInvestmentItems",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmInvestmentItems_PosmCatalogId",
                table: "PosmInvestmentItems",
                column: "PosmCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmInvestmentItems_PosmClassId",
                table: "PosmInvestmentItems",
                column: "PosmClassId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmInvestmentItems_PosmInvestmentId",
                table: "PosmInvestmentItems",
                column: "PosmInvestmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmInvestmentItems_PosmItemId",
                table: "PosmInvestmentItems",
                column: "PosmItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmInvestments_BudgetId",
                table: "PosmInvestments",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmInvestments_Code",
                table: "PosmInvestments",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_PosmInvestments_CreatorUserId",
                table: "PosmInvestments",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmInvestments_CustomerId",
                table: "PosmInvestments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmInvestments_CustomerLocationId",
                table: "PosmInvestments",
                column: "CustomerLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmInvestments_CycleId",
                table: "PosmInvestments",
                column: "CycleId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmInvestments_LastModifierUserId",
                table: "PosmInvestments",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmInvestments_RegisterStaffId",
                table: "PosmInvestments",
                column: "RegisterStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmInvestments_Status_CreationTime",
                table: "PosmInvestments",
                columns: new[] { "Status", "CreationTime" });

            migrationBuilder.CreateIndex(
                name: "IX_PosmSalesCommitments_CreatorUserId",
                table: "PosmSalesCommitments",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmSalesCommitments_LastModifierUserId",
                table: "PosmSalesCommitments",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmSalesCommitments_PosmInvestmentId",
                table: "PosmSalesCommitments",
                column: "PosmInvestmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PosmInvestmentItemHistories");

            migrationBuilder.DropTable(
                name: "PosmSalesCommitments");

            migrationBuilder.DropTable(
                name: "PosmInvestmentItems");

            migrationBuilder.DropTable(
                name: "PosmInvestments");

            migrationBuilder.DropSequence(
                name: "posm_investment_item_history_seq");

            migrationBuilder.DropSequence(
                name: "posm_investment_item_seq");

            migrationBuilder.DropSequence(
                name: "posm_investment_seq");

            migrationBuilder.DropSequence(
                name: "posm_sales_commitment_seq");

            migrationBuilder.AddColumn<decimal>(
                name: "Height",
                table: "PosmItems",
                type: "DECIMAL(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Qty",
                table: "PosmItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SideWidth1",
                table: "PosmItems",
                type: "DECIMAL(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SideWidth2",
                table: "PosmItems",
                type: "DECIMAL(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitValue",
                table: "PosmItems",
                type: "DECIMAL(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Width",
                table: "PosmItems",
                type: "DECIMAL(18,2)",
                nullable: true);
        }
    }
}
