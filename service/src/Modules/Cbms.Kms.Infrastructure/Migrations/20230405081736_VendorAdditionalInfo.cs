using Microsoft.EntityFrameworkCore.Migrations;

namespace Cbms.Kms.Infrastructure.Migrations
{
    public partial class VendorAdditionalInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Representative",
                table: "Vendors",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxReg",
                table: "Vendors",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ZoneId",
                table: "Vendors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_ZoneId",
                table: "Vendors",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Zones_ZoneId",
                table: "Vendors",
                column: "ZoneId",
                principalTable: "Zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Zones_ZoneId",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_ZoneId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Representative",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "TaxReg",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "Vendors");
        }
    }
}
