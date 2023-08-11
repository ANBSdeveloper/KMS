using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cbms.Kms.Infrastructure.Migrations
{
    public partial class PosmType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "posm_type_seq");

            migrationBuilder.AddColumn<int>(
                name: "PosmTypeId",
                table: "PosmItems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PosmTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosmTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PosmTypes_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmTypes_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PosmTypes_Code",
                table: "PosmTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PosmTypes_CreatorUserId",
                table: "PosmTypes",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmTypes_LastModifierUserId",
                table: "PosmTypes",
                column: "LastModifierUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PosmTypes");

            migrationBuilder.DropSequence(
                name: "posm_type_seq");

            migrationBuilder.DropColumn(
                name: "PosmTypeId",
                table: "PosmItems");
        }
    }
}
