using Microsoft.EntityFrameworkCore.Migrations;

namespace Cbms.Kms.Infrastructure.Migrations
{
    public partial class UserAssignmentUpdateIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserAssignments_UserId_SalesOrgId",
                table: "UserAssignments");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssignments_UserId_SalesOrgId",
                table: "UserAssignments",
                columns: new[] { "UserId", "SalesOrgId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserAssignments_UserId_SalesOrgId",
                table: "UserAssignments");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssignments_UserId_SalesOrgId",
                table: "UserAssignments",
                columns: new[] { "UserId", "SalesOrgId" },
                unique: true);
        }
    }
}
