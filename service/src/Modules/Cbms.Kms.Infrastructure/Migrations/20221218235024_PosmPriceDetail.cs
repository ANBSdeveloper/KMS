using Microsoft.EntityFrameworkCore.Migrations;

namespace Cbms.Kms.Infrastructure.Migrations
{
    public partial class PosmPriceDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PosmPriceDetails_PosmItemId",
                table: "PosmPriceDetails");

            migrationBuilder.CreateIndex(
                name: "IX_PosmPriceDetails_PosmItemId",
                table: "PosmPriceDetails",
                column: "PosmItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PosmPriceDetails_PosmItemId",
                table: "PosmPriceDetails");

            migrationBuilder.CreateIndex(
                name: "IX_PosmPriceDetails_PosmItemId",
                table: "PosmPriceDetails",
                column: "PosmItemId",
                unique: true);
        }
    }
}
