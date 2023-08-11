using Microsoft.EntityFrameworkCore.Migrations;

namespace Cbms.Kms.Infrastructure.Migrations
{
    public partial class PosmClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IncludeInfo",
                table: "PosmClasses",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncludeInfo",
                table: "PosmClasses");
        }
    }
}
