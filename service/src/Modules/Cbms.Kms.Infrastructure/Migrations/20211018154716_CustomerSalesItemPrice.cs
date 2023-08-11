using Microsoft.EntityFrameworkCore.Migrations;

namespace Cbms.Kms.Infrastructure.Migrations
{
    public partial class CustomerSalesItemPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "CustomerSalesItems",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "CustomerSalesItems");
        }
    }
}
