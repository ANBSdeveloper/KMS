using Microsoft.EntityFrameworkCore.Migrations;

namespace Cbms.Kms.Infrastructure.Migrations
{
    public partial class Remark : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RemarkOfCompany",
                table: "PosmInvestmentItems",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RemarkOfSales",
                table: "PosmInvestmentItems",
                type: "decimal(10,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemarkOfCompany",
                table: "PosmInvestmentItems");

            migrationBuilder.DropColumn(
                name: "RemarkOfSales",
                table: "PosmInvestmentItems");
        }
    }
}
