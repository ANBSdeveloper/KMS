using Microsoft.EntityFrameworkCore.Migrations;

namespace Cbms.Kms.Infrastructure.Migrations
{
    public partial class Edit_Unique_Reward : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RewardItems_RewardPackageId",
                table: "RewardItems");

            migrationBuilder.DropIndex(
                name: "IX_Branches_Code",
                table: "Branches");

            migrationBuilder.CreateIndex(
                name: "IX_RewardPackages_Code",
                table: "RewardPackages",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RewardItems_RewardPackageId_Code",
                table: "RewardItems",
                columns: new[] { "RewardPackageId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_Code",
                table: "Branches",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RewardPackages_Code",
                table: "RewardPackages");

            migrationBuilder.DropIndex(
                name: "IX_RewardItems_RewardPackageId_Code",
                table: "RewardItems");

            migrationBuilder.DropIndex(
                name: "IX_Branches_Code",
                table: "Branches");

            migrationBuilder.CreateIndex(
                name: "IX_RewardItems_RewardPackageId",
                table: "RewardItems",
                column: "RewardPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_Code",
                table: "Branches",
                column: "Code");
        }
    }
}
