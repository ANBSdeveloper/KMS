using Microsoft.EntityFrameworkCore.Migrations;

namespace Cbms.Kms.Infrastructure.Migrations
{
    public partial class PosmHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PosmInvestmentItemHistories_PosmInvestmentItems_PosmInvestmentItemId",
                table: "PosmInvestmentItemHistories");

            migrationBuilder.DropColumn(
                name: "PostInvestmentItemId",
                table: "PosmInvestmentItemHistories");

            migrationBuilder.AlterColumn<int>(
                name: "PosmInvestmentItemId",
                table: "PosmInvestmentItemHistories",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PosmInvestmentItemHistories_PosmInvestmentItems_PosmInvestmentItemId",
                table: "PosmInvestmentItemHistories",
                column: "PosmInvestmentItemId",
                principalTable: "PosmInvestmentItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PosmInvestmentItemHistories_PosmInvestmentItems_PosmInvestmentItemId",
                table: "PosmInvestmentItemHistories");

            migrationBuilder.AlterColumn<int>(
                name: "PosmInvestmentItemId",
                table: "PosmInvestmentItemHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "PostInvestmentItemId",
                table: "PosmInvestmentItemHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_PosmInvestmentItemHistories_PosmInvestmentItems_PosmInvestmentItemId",
                table: "PosmInvestmentItemHistories",
                column: "PosmInvestmentItemId",
                principalTable: "PosmInvestmentItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
