using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cbms.Kms.Infrastructure.Migrations
{
    public partial class Posm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "posm_catalog_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "posm_class_seq");

            migrationBuilder.CreateSequence(
                name: "posm_item_seq");

            migrationBuilder.CreateTable(
                name: "PosmClasses",
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
                    table.PrimaryKey("PK_PosmClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PosmClasses_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmClasses_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PosmItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    PosmClassId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    UnitType = table.Column<int>(nullable: false),
                    CalcType = table.Column<int>(nullable: false),
                    Width = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: true),
                    Height = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: true),
                    SideWidth1 = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: true),
                    SideWidth2 = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: true),
                    Qty = table.Column<int>(nullable: true),
                    UnitValue = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosmItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PosmItems_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmItems_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmItems_PosmClasses_PosmClassId",
                        column: x => x.PosmClassId,
                        principalTable: "PosmClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PosmCatalogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    PosmItemId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Link = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosmCatalogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PosmCatalogs_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmCatalogs_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosmCatalogs_PosmItems_PosmItemId",
                        column: x => x.PosmItemId,
                        principalTable: "PosmItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PosmCatalogs_CreatorUserId",
                table: "PosmCatalogs",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmCatalogs_LastModifierUserId",
                table: "PosmCatalogs",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmCatalogs_PosmItemId_Code",
                table: "PosmCatalogs",
                columns: new[] { "PosmItemId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PosmClasses_Code",
                table: "PosmClasses",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PosmClasses_CreatorUserId",
                table: "PosmClasses",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmClasses_LastModifierUserId",
                table: "PosmClasses",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmItems_Code",
                table: "PosmItems",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PosmItems_CreatorUserId",
                table: "PosmItems",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmItems_LastModifierUserId",
                table: "PosmItems",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PosmItems_PosmClassId",
                table: "PosmItems",
                column: "PosmClassId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PosmCatalogs");

            migrationBuilder.DropTable(
                name: "PosmItems");

            migrationBuilder.DropTable(
                name: "PosmClasses");

            migrationBuilder.DropSequence(
                name: "posm_catalog_seq");

            migrationBuilder.DropSequence(
                name: "posm_class_seq");

            migrationBuilder.DropSequence(
                name: "posm_item_seq");
        }
    }
}
