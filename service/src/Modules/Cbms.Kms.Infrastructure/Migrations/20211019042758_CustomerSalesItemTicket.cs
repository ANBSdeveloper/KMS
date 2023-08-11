using Microsoft.EntityFrameworkCore.Migrations;

namespace Cbms.Kms.Infrastructure.Migrations
{
    public partial class CustomerSalesItemTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketInvestmentId",
                table: "CustomerSalesItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSalesItems_TicketInvestmentId",
                table: "CustomerSalesItems",
                column: "TicketInvestmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSalesItems_TicketInvestments_TicketInvestmentId",
                table: "CustomerSalesItems",
                column: "TicketInvestmentId",
                principalTable: "TicketInvestments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerSalesItems_TicketInvestments_TicketInvestmentId",
                table: "CustomerSalesItems");

            migrationBuilder.DropIndex(
                name: "IX_CustomerSalesItems_TicketInvestmentId",
                table: "CustomerSalesItems");

            migrationBuilder.DropColumn(
                name: "TicketInvestmentId",
                table: "CustomerSalesItems");
        }
    }
}
