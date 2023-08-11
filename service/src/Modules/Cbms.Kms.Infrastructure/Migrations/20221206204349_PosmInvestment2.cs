using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cbms.Kms.Infrastructure.Migrations
{
    public partial class PosmInvestment2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalActualCost",
                table: "PosmInvestmentItems");

            migrationBuilder.AddColumn<string>(
                name: "CancelReason",
                table: "PosmInvestments",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ActualTotalCost",
                table: "PosmInvestmentItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ActualUnitPrice",
                table: "PosmInvestmentItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PrepareDate",
                table: "PosmInvestmentItems",
                type: "datetime2(0)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrepareNote",
                table: "PosmInvestmentItems",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateCostReason",
                table: "PosmInvestmentItems",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "PosmInvestmentItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PosmInvestmentItems_VendorId",
                table: "PosmInvestmentItems",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PosmInvestmentItems_Vendors_VendorId",
                table: "PosmInvestmentItems",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PosmInvestmentItems_Vendors_VendorId",
                table: "PosmInvestmentItems");

            migrationBuilder.DropIndex(
                name: "IX_PosmInvestmentItems_VendorId",
                table: "PosmInvestmentItems");

            migrationBuilder.DropColumn(
                name: "CancelReason",
                table: "PosmInvestments");

            migrationBuilder.DropColumn(
                name: "ActualTotalCost",
                table: "PosmInvestmentItems");

            migrationBuilder.DropColumn(
                name: "ActualUnitPrice",
                table: "PosmInvestmentItems");

            migrationBuilder.DropColumn(
                name: "PrepareDate",
                table: "PosmInvestmentItems");

            migrationBuilder.DropColumn(
                name: "PrepareNote",
                table: "PosmInvestmentItems");

            migrationBuilder.DropColumn(
                name: "UpdateCostReason",
                table: "PosmInvestmentItems");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "PosmInvestmentItems");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalActualCost",
                table: "PosmInvestmentItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
