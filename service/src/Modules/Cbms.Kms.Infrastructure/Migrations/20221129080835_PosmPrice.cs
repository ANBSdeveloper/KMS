using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cbms.Kms.Infrastructure.Migrations
{
    public partial class PosmPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "posm_price_detail",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "posm_price_header_seq");

            migrationBuilder.CreateTable(
                name: "PosmPriceHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosmPriceHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PosmPriceHeaders_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmPriceHeaders_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PosmPriceDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    PosmPriceHeaderId = table.Column<int>(nullable: false),
                    PosmItemId = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosmPriceDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PosmPriceDetails_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmPriceDetails_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmPriceDetails_PosmItems_PosmItemId",
                        column: x => x.PosmItemId,
                        principalTable: "PosmItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmPriceDetails_PosmPriceHeaders_PosmPriceHeaderId",
                        column: x => x.PosmPriceHeaderId,
                        principalTable: "PosmPriceHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PosmPriceDetails_CreatorUserId",
                table: "PosmPriceDetails",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmPriceDetails_LastModifierUserId",
                table: "PosmPriceDetails",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmPriceDetails_PosmItemId",
                table: "PosmPriceDetails",
                column: "PosmItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PosmPriceDetails_PosmPriceHeaderId",
                table: "PosmPriceDetails",
                column: "PosmPriceHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmPriceHeaders_Code",
                table: "PosmPriceHeaders",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PosmPriceHeaders_CreatorUserId",
                table: "PosmPriceHeaders",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmPriceHeaders_LastModifierUserId",
                table: "PosmPriceHeaders",
                column: "LastModifierUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PosmPriceDetails");

            migrationBuilder.DropTable(
                name: "PosmPriceHeaders");

            migrationBuilder.DropSequence(
                name: "posm_price_detail");

            migrationBuilder.DropSequence(
                name: "posm_price_header_seq");
        }
    }
}
