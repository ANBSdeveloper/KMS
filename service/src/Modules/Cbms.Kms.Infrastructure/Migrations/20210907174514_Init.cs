using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cbms.Kms.Infrastructure.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "app_setting_seq");

            migrationBuilder.CreateSequence(
                name: "area_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "branch_seq");

            migrationBuilder.CreateSequence(
                name: "brand_seq");

            migrationBuilder.CreateSequence(
                name: "budget_detail_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "budget_seq");

            migrationBuilder.CreateSequence(
                name: "channel_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "consumer_seq");

            migrationBuilder.CreateSequence(
                name: "customer_sale_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "customer_sales_item_seq");

            migrationBuilder.CreateSequence(
                name: "customer_seq",
                incrementBy: 100);

            migrationBuilder.CreateSequence(
                name: "cycle_seq");

            migrationBuilder.CreateSequence(
                name: "district_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "investment_branch_setting_seq");

            migrationBuilder.CreateSequence(
                name: "investmentSetting_seq");

            migrationBuilder.CreateSequence(
                name: "material_seq");

            migrationBuilder.CreateSequence(
                name: "material_type_seq");

            migrationBuilder.CreateSequence(
                name: "notification_branch_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "notification_seq");

            migrationBuilder.CreateSequence(
                name: "notification_user_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "order_detail_seq",
                incrementBy: 5);

            migrationBuilder.CreateSequence(
                name: "order_seq");

            migrationBuilder.CreateSequence(
                name: "order_ticket_seq",
                incrementBy: 5);

            migrationBuilder.CreateSequence(
                name: "permission_seq");

            migrationBuilder.CreateSequence(
                name: "product_class_seq");

            migrationBuilder.CreateSequence(
                name: "product_point_seq");

            migrationBuilder.CreateSequence(
                name: "product_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "product_unit_seq");

            migrationBuilder.CreateSequence(
                name: "province_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "reward_branch_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "reward_item_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "reward_package_seq");

            migrationBuilder.CreateSequence(
                name: "role_permission_seq");

            migrationBuilder.CreateSequence(
                name: "role_seq");

            migrationBuilder.CreateSequence(
                name: "staff_seq",
                incrementBy: 100);

            migrationBuilder.CreateSequence(
                name: "sub_product_class_seq");

            migrationBuilder.CreateSequence(
                name: "ticket_acceptance_seq");

            migrationBuilder.CreateSequence(
                name: "ticket_customer_reward_detail_seq",
                incrementBy: 5);

            migrationBuilder.CreateSequence(
                name: "ticket_customer_reward_seq");

            migrationBuilder.CreateSequence(
                name: "ticket_final_settlement_seq");

            migrationBuilder.CreateSequence(
                name: "ticket_investment_history_seq");

            migrationBuilder.CreateSequence(
                name: "ticket_investment_seq");

            migrationBuilder.CreateSequence(
                name: "ticket_material_seq",
                incrementBy: 5);

            migrationBuilder.CreateSequence(
                name: "ticket_operation_seq");

            migrationBuilder.CreateSequence(
                name: "ticket_progress_material_seq");

            migrationBuilder.CreateSequence(
                name: "ticket_progress_reward_item_seq");

            migrationBuilder.CreateSequence(
                name: "ticket_progress_seq");

            migrationBuilder.CreateSequence(
                name: "ticket_reward_item_seq",
                incrementBy: 5);

            migrationBuilder.CreateSequence(
                name: "ticket_sales_commitment_seq",
                incrementBy: 5);

            migrationBuilder.CreateSequence(
                name: "ticket_seq",
                incrementBy: 5);

            migrationBuilder.CreateSequence(
                name: "user_assignment_seq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "user_reset_ticket_seq");

            migrationBuilder.CreateSequence(
                name: "user_role_seq");

            migrationBuilder.CreateSequence(
                name: "user_seq");

            migrationBuilder.CreateSequence(
                name: "ward_seq",
                incrementBy: 100);

            migrationBuilder.CreateSequence(
                name: "zone_seq",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "AppLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Data = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    AuthenticationSource = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    UserName = table.Column<string>(maxLength: 36, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Password = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    EmailAddress = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    RegisterDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "GetDate()"),
                    ExpireDate = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Value = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSettings_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppSettings_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Brands_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Brands_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    SalesOrgId = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Channels_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Channels_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Consumers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Phone = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    OtpCode = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    OtpTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consumers_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Consumers_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cycles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Number = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cycles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cycles_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cycles_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentSetting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    MaxInvestAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountPerPoint = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxInvestmentQueryMonths = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CheckQrCodeBranch = table.Column<bool>(nullable: false),
                    DefaultPointsForTicket = table.Column<decimal>(type: "decimal(8,2)", nullable: false, defaultValue: 0m),
                    BeginIssueDaysAfterCurrent = table.Column<int>(nullable: false),
                    EndIssueDaysBeforeOperation = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentSetting_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvestmentSetting_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialTypes",
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
                    table.PrimaryKey("PK_MaterialTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialTypes_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialTypes_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ObjectType = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: false),
                    ShortContent = table.Column<string>(maxLength: 500, nullable: false),
                    Content = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 100, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permissions_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductClasses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    RewardCode = table.Column<string>(maxLength: 50, nullable: false, defaultValue: ""),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductClasses_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductClasses_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductUnits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductUnits_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductUnits_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Provinces_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Provinces_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RewardPackages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    TotalTickets = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RewardPackages_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RewardPackages_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    RoleName = table.Column<string>(maxLength: 100, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Roles_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrgs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                    TypeName = table.Column<string>(maxLength: 200, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrgs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesOrgs_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesOrgs_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubProductClasses",
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
                    table.PrimaryKey("PK_SubProductClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubProductClasses_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubProductClasses_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserResetTickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Token = table.Column<string>(unicode: false, maxLength: 200, nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    IsUsed = table.Column<bool>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserResetTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserResetTickets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    SalesOrgId = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zones_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Zones_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    CycleId = table.Column<int>(nullable: false),
                    InvestmentType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Budgets_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Budgets_Cycles_CycleId",
                        column: x => x.CycleId,
                        principalTable: "Cycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Budgets_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDesign = table.Column<bool>(nullable: false),
                    MaterialTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materials_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Materials_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Materials_MaterialTypes_MaterialTypeId",
                        column: x => x.MaterialTypeId,
                        principalTable: "MaterialTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ViewDate = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    NotificationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationUsers_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    ProvinceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Districts_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Districts_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    PermissionId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    SalesOrgId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAssignments_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAssignments_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAssignments_SalesOrgs_SalesOrgId",
                        column: x => x.SalesOrgId,
                        principalTable: "SalesOrgs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAssignments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Unit = table.Column<string>(maxLength: 50, nullable: true),
                    CaseUnit = table.Column<string>(maxLength: 50, nullable: true),
                    PackSize = table.Column<int>(nullable: false),
                    ProductClassId = table.Column<int>(nullable: true),
                    SubProductClassId = table.Column<int>(nullable: true),
                    BrandId = table.Column<int>(nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_ProductClasses_ProductClassId",
                        column: x => x.ProductClassId,
                        principalTable: "ProductClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_SubProductClasses_SubProductClassId",
                        column: x => x.SubProductClassId,
                        principalTable: "SubProductClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    ZoneId = table.Column<int>(nullable: false),
                    SalesOrgId = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Areas_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Areas_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Areas_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    DistrictId = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wards_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Wards_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Wards_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductPoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    Points = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPoints_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductPoints_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductPoints_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RewardItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    RewardPackageId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    DocumentLink = table.Column<string>(unicode: false, maxLength: 1000, nullable: true),
                    ProductUnitId = table.Column<int>(nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RewardItems_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RewardItems_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RewardItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RewardItems_ProductUnits_ProductUnitId",
                        column: x => x.ProductUnitId,
                        principalTable: "ProductUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RewardItems_RewardPackages_RewardPackageId",
                        column: x => x.RewardPackageId,
                        principalTable: "RewardPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    SalesOrgId = table.Column<int>(nullable: false),
                    AreaId = table.Column<int>(nullable: true),
                    ZoneId = table.Column<int>(nullable: true),
                    ChannelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Branches_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Branches_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Branches_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Branches_SalesOrgs_SalesOrgId",
                        column: x => x.SalesOrgId,
                        principalTable: "SalesOrgs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Branches_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    SalesOrgId = table.Column<int>(nullable: false),
                    StaffTypeCode = table.Column<string>(maxLength: 100, nullable: false),
                    StaffTypeName = table.Column<string>(maxLength: 200, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    MobilePhone = table.Column<string>(maxLength: 20, nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    AreaId = table.Column<int>(nullable: true),
                    ZoneId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staffs_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Staffs_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Staffs_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Staffs_SalesOrgs_SalesOrgId",
                        column: x => x.SalesOrgId,
                        principalTable: "SalesOrgs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Staffs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Staffs_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentBranchSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsEditablePoint = table.Column<bool>(nullable: false),
                    BranchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentBranchSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentBranchSettings_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvestmentBranchSettings_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvestmentBranchSettings_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationBranches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    BranchId = table.Column<int>(nullable: false),
                    NotificationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationBranches_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationBranches_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationBranches_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationBranches_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RewardBranches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    RewardPackageId = table.Column<int>(nullable: false),
                    BranchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RewardBranches_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RewardBranches_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RewardBranches_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RewardBranches_RewardPackages_RewardPackageId",
                        column: x => x.RewardPackageId,
                        principalTable: "RewardPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    StaffId = table.Column<int>(nullable: false),
                    BudgetId = table.Column<int>(nullable: false),
                    AllocateAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    TempUsedAmount = table.Column<decimal>(nullable: false),
                    TempRemainAmount = table.Column<decimal>(nullable: false),
                    UsedAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    RemainAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetDetails_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetDetails_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetDetails_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetDetails_Staffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    BranchId = table.Column<int>(nullable: true),
                    ContactName = table.Column<string>(maxLength: 200, nullable: true),
                    MobilePhone = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    Phone = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    Email = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    ChannelCode = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    ChannelName = table.Column<string>(maxLength: 200, nullable: true),
                    HouseNumber = table.Column<string>(maxLength: 200, nullable: true),
                    Street = table.Column<string>(maxLength: 100, nullable: true),
                    Address = table.Column<string>(maxLength: 500, nullable: true),
                    WardId = table.Column<int>(nullable: true),
                    DistrictId = table.Column<int>(nullable: true),
                    ProvinceId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Lat = table.Column<float>(nullable: false),
                    Lng = table.Column<float>(nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    AreaId = table.Column<int>(nullable: true),
                    ZoneId = table.Column<int>(nullable: true),
                    IsKeyShop = table.Column<bool>(nullable: false, defaultValue: false),
                    IsAllowKeyShopRegister = table.Column<bool>(nullable: false, defaultValue: true),
                    KeyShopStatus = table.Column<int>(nullable: false, defaultValue: 10),
                    KeyShopAuthCode = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    Efficient = table.Column<decimal>(nullable: true),
                    OtpCode = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    OtpTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    KeyShopRegisterStaffId = table.Column<int>(nullable: true),
                    RsmStaffId = table.Column<int>(nullable: true),
                    AsmStaffId = table.Column<int>(nullable: true),
                    SalesSupervisorStaffId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_Staffs_KeyShopRegisterStaffId",
                        column: x => x.KeyShopRegisterStaffId,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    Year = table.Column<string>(unicode: false, maxLength: 4, nullable: false),
                    Month = table.Column<string>(unicode: false, maxLength: 2, nullable: false),
                    YearMonth = table.Column<string>(unicode: false, maxLength: 6, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerSales_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerSales_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerSales_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSalesItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    QrCode = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSalesItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerSalesItems_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerSalesItems_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerSalesItems_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerSalesItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductPrices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: true),
                    SalesOrgId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PackagePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPrices_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductPrices_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductPrices_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductPrices_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductPrices_SalesOrgs_SalesOrgId",
                        column: x => x.SalesOrgId,
                        principalTable: "SalesOrgs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketInvestments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    RegisterStaffId = table.Column<int>(nullable: false),
                    StockQuantity = table.Column<int>(nullable: false),
                    RewardPackageId = table.Column<int>(nullable: false),
                    BudgetId = table.Column<int>(nullable: false),
                    CycleId = table.Column<int>(nullable: false),
                    TicketQuantity = table.Column<int>(nullable: false),
                    PointsForTicket = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalesPlanAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    CommitmentAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    RewardAmount = table.Column<decimal>(nullable: false),
                    MaterialAmount = table.Column<decimal>(nullable: false),
                    InvestmentAmount = table.Column<decimal>(nullable: false),
                    PrintTicketQuantity = table.Column<int>(nullable: false),
                    SmsTicketQuantity = table.Column<int>(nullable: false),
                    ActualSalesAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    BuyBeginDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    BuyEndDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    IssueTicketBeginDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    IssueTicketEndDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    Status = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    OperationDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    RegisterNote = table.Column<string>(nullable: true),
                    SurveyPhoto1 = table.Column<string>(nullable: true),
                    SurveyPhoto2 = table.Column<string>(nullable: true),
                    SurveyPhoto3 = table.Column<string>(nullable: true),
                    SurveyPhoto4 = table.Column<string>(nullable: true),
                    SurveyPhoto5 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketInvestments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketInvestments_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketInvestments_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketInvestments_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketInvestments_Cycles_CycleId",
                        column: x => x.CycleId,
                        principalTable: "Cycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketInvestments_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketInvestments_Staffs_RegisterStaffId",
                        column: x => x.RegisterStaffId,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketInvestments_RewardPackages_RewardPackageId",
                        column: x => x.RewardPackageId,
                        principalTable: "RewardPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    OrderNumber = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    TicketInvestmentId = table.Column<int>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    BranchId = table.Column<int>(nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    ConsumerPhone = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    ConsumerName = table.Column<string>(maxLength: 200, nullable: true),
                    TotalQuantity = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    TotalPoints = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    TotalAvailablePoints = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    TotalUsedPoints = table.Column<decimal>(type: "decimal(8,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_TicketInvestments_TicketInvestmentId",
                        column: x => x.TicketInvestmentId,
                        principalTable: "TicketInvestments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketAcceptances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    AcceptanceDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    Photo1 = table.Column<string>(nullable: true),
                    Photo2 = table.Column<string>(nullable: true),
                    Photo3 = table.Column<string>(nullable: true),
                    Photo4 = table.Column<string>(nullable: true),
                    Photo5 = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    RemarkOfSales = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    RemarkOfCompany = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    RemarkOfCustomerDevelopement = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    UpdateUserId = table.Column<int>(nullable: false),
                    TicketInvestmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketAcceptances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketAcceptances_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketAcceptances_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketAcceptances_TicketInvestments_TicketInvestmentId",
                        column: x => x.TicketInvestmentId,
                        principalTable: "TicketInvestments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketAcceptances_Users_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketConsumerRewards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    RewardItemId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    RewardQuantity = table.Column<int>(nullable: false),
                    Photo1 = table.Column<string>(nullable: true),
                    Photo2 = table.Column<string>(nullable: true),
                    Photo3 = table.Column<string>(nullable: true),
                    Photo4 = table.Column<string>(nullable: true),
                    Photo5 = table.Column<string>(nullable: true),
                    TicketInvestmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketConsumerRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketConsumerRewards_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketConsumerRewards_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketConsumerRewards_RewardItems_RewardItemId",
                        column: x => x.RewardItemId,
                        principalTable: "RewardItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketConsumerRewards_TicketInvestments_TicketInvestmentId",
                        column: x => x.TicketInvestmentId,
                        principalTable: "TicketInvestments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketFinalSettlements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    Note = table.Column<string>(nullable: true),
                    TicketInvestmentId = table.Column<int>(nullable: false),
                    DecideUserId = table.Column<int>(nullable: false),
                    UpdateUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketFinalSettlements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketFinalSettlements_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketFinalSettlements_Users_DecideUserId",
                        column: x => x.DecideUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketFinalSettlements_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketFinalSettlements_TicketInvestments_TicketInvestmentId",
                        column: x => x.TicketInvestmentId,
                        principalTable: "TicketInvestments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketFinalSettlements_Users_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketInvestmentHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    TicketInvestmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketInvestmentHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketInvestmentHistories_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketInvestmentHistories_TicketInvestments_TicketInvestmentId",
                        column: x => x.TicketInvestmentId,
                        principalTable: "TicketInvestments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    MaterialId = table.Column<int>(nullable: false),
                    RegisterQuantity = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TicketInvestmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketMaterials_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketMaterials_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketMaterials_TicketInvestments_TicketInvestmentId",
                        column: x => x.TicketInvestmentId,
                        principalTable: "TicketInvestments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketOperations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    OperationDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    Note = table.Column<string>(nullable: true),
                    StockQuantity = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    Photo1 = table.Column<string>(nullable: true),
                    Photo2 = table.Column<string>(nullable: true),
                    Photo3 = table.Column<string>(nullable: true),
                    Photo4 = table.Column<string>(nullable: true),
                    Photo5 = table.Column<string>(nullable: true),
                    TicketInvestmentId = table.Column<int>(nullable: false),
                    UpdateUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketOperations_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketOperations_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketOperations_TicketInvestments_TicketInvestmentId",
                        column: x => x.TicketInvestmentId,
                        principalTable: "TicketInvestments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketOperations_Users_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    DocumentPhoto1 = table.Column<string>(nullable: true),
                    DocumentPhoto2 = table.Column<string>(nullable: true),
                    DocumentPhoto3 = table.Column<string>(nullable: true),
                    DocumentPhoto4 = table.Column<string>(nullable: true),
                    DocumentPhoto5 = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    UpdateUserId = table.Column<int>(nullable: false),
                    TicketInvestmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketProgresses_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketProgresses_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketProgresses_TicketInvestments_TicketInvestmentId",
                        column: x => x.TicketInvestmentId,
                        principalTable: "TicketInvestments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketProgresses_Users_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketRewardItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    RewardItemId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    TicketInvestmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketRewardItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketRewardItems_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketRewardItems_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketRewardItems_RewardItems_RewardItemId",
                        column: x => x.RewardItemId,
                        principalTable: "RewardItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketRewardItems_TicketInvestments_TicketInvestmentId",
                        column: x => x.TicketInvestmentId,
                        principalTable: "TicketInvestments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Code = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    ConsumerPhone = table.Column<string>(maxLength: 2000, nullable: true),
                    ConsumerName = table.Column<string>(maxLength: 200, nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    PrintDate = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    PrintCount = table.Column<int>(nullable: false),
                    LastPrintUserId = table.Column<int>(nullable: true),
                    TicketInvestmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_LastPrintUserId",
                        column: x => x.LastPrintUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketInvestments_TicketInvestmentId",
                        column: x => x.TicketInvestmentId,
                        principalTable: "TicketInvestments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketSalesCommitments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    TicketInvestmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketSalesCommitments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketSalesCommitments_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketSalesCommitments_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketSalesCommitments_TicketInvestments_TicketInvestmentId",
                        column: x => x.TicketInvestmentId,
                        principalTable: "TicketInvestments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(nullable: true),
                    QrCode = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    SpoonCode = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    ProductName = table.Column<string>(maxLength: 200, nullable: true),
                    UnitName = table.Column<string>(maxLength: 50, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    Api = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    Points = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    AvailablePoints = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    UsedPoints = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    UsedForTicket = table.Column<bool>(nullable: false),
                    OrderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketProgressMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    MaterialId = table.Column<int>(nullable: false),
                    IsReceived = table.Column<bool>(nullable: false),
                    IsSentDesign = table.Column<bool>(nullable: false),
                    Photo1 = table.Column<string>(nullable: true),
                    Photo2 = table.Column<string>(nullable: true),
                    Photo3 = table.Column<string>(nullable: true),
                    Photo4 = table.Column<string>(nullable: true),
                    Photo5 = table.Column<string>(nullable: true),
                    TicketProgressId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketProgressMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketProgressMaterials_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketProgressMaterials_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketProgressMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketProgressMaterials_TicketProgresses_TicketProgressId",
                        column: x => x.TicketProgressId,
                        principalTable: "TicketProgresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketProgressRewardItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    RewardItemId = table.Column<int>(nullable: false),
                    IsReceived = table.Column<bool>(nullable: false),
                    TicketProgressId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketProgressRewardItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketProgressRewardItems_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketProgressRewardItems_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketProgressRewardItems_RewardItems_RewardItemId",
                        column: x => x.RewardItemId,
                        principalTable: "RewardItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketProgressRewardItems_TicketProgresses_TicketProgressId",
                        column: x => x.TicketProgressId,
                        principalTable: "TicketProgresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderTickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    TicketId = table.Column<int>(nullable: false),
                    TicketCode = table.Column<string>(nullable: true),
                    OrderId = table.Column<int>(nullable: false),
                    QrCode = table.Column<string>(unicode: false, maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTickets_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderTickets_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderTickets_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderTickets_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketConsumerRewardDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    TicketId = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    TicketConsumerRewardId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketConsumerRewardDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketConsumerRewardDetails_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketConsumerRewardDetails_Users_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketConsumerRewardDetails_TicketConsumerRewards_TicketConsumerRewardId",
                        column: x => x.TicketConsumerRewardId,
                        principalTable: "TicketConsumerRewards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketConsumerRewardDetails_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppLogs_Name",
                table: "AppLogs",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AppSettings_Code",
                table: "AppSettings",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_AppSettings_CreatorUserId",
                table: "AppSettings",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSettings_LastModifierUserId",
                table: "AppSettings",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_Code",
                table: "Areas",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Areas_CreatorUserId",
                table: "Areas",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_LastModifierUserId",
                table: "Areas",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_ZoneId",
                table: "Areas",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_AreaId",
                table: "Branches",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_ChannelId",
                table: "Branches",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_Code",
                table: "Branches",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CreatorUserId",
                table: "Branches",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_LastModifierUserId",
                table: "Branches",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_SalesOrgId",
                table: "Branches",
                column: "SalesOrgId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_ZoneId",
                table: "Branches",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Code",
                table: "Brands",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_CreatorUserId",
                table: "Brands",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_LastModifierUserId",
                table: "Brands",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_BudgetId",
                table: "BudgetDetails",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_CreatorUserId",
                table: "BudgetDetails",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_LastModifierUserId",
                table: "BudgetDetails",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_StaffId",
                table: "BudgetDetails",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_CreatorUserId",
                table: "Budgets",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_CycleId",
                table: "Budgets",
                column: "CycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_LastModifierUserId",
                table: "Budgets",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Channels_Code",
                table: "Channels",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channels_CreatorUserId",
                table: "Channels",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Channels_LastModifierUserId",
                table: "Channels",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Consumers_CreatorUserId",
                table: "Consumers",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Consumers_LastModifierUserId",
                table: "Consumers",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Consumers_Phone",
                table: "Consumers",
                column: "Phone",
                unique: true,
                filter: "[Phone] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AreaId",
                table: "Customers",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_BranchId",
                table: "Customers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CreatorUserId",
                table: "Customers",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_DistrictId",
                table: "Customers",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_KeyShopRegisterStaffId",
                table: "Customers",
                column: "KeyShopRegisterStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LastModifierUserId",
                table: "Customers",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ProvinceId",
                table: "Customers",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_WardId",
                table: "Customers",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ZoneId",
                table: "Customers",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSales_CreatorUserId",
                table: "CustomerSales",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSales_LastModifierUserId",
                table: "CustomerSales",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSales_CustomerId_YearMonth",
                table: "CustomerSales",
                columns: new[] { "CustomerId", "YearMonth" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSalesItems_CreatorUserId",
                table: "CustomerSalesItems",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSalesItems_CustomerId",
                table: "CustomerSalesItems",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSalesItems_LastModifierUserId",
                table: "CustomerSalesItems",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSalesItems_ProductId",
                table: "CustomerSalesItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSalesItems_QrCode",
                table: "CustomerSalesItems",
                column: "QrCode",
                unique: true,
                filter: "[QrCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cycles_CreatorUserId",
                table: "Cycles",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cycles_LastModifierUserId",
                table: "Cycles",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cycles_Number",
                table: "Cycles",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Districts_Code",
                table: "Districts",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CreatorUserId",
                table: "Districts",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_LastModifierUserId",
                table: "Districts",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_ProvinceId",
                table: "Districts",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentBranchSettings_BranchId",
                table: "InvestmentBranchSettings",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentBranchSettings_CreatorUserId",
                table: "InvestmentBranchSettings",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentBranchSettings_LastModifierUserId",
                table: "InvestmentBranchSettings",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentSetting_CreatorUserId",
                table: "InvestmentSetting",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentSetting_LastModifierUserId",
                table: "InvestmentSetting",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Code",
                table: "Materials",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_CreatorUserId",
                table: "Materials",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_LastModifierUserId",
                table: "Materials",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MaterialTypeId",
                table: "Materials",
                column: "MaterialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTypes_Code",
                table: "MaterialTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTypes_CreatorUserId",
                table: "MaterialTypes",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTypes_LastModifierUserId",
                table: "MaterialTypes",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationBranches_BranchId",
                table: "NotificationBranches",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationBranches_CreatorUserId",
                table: "NotificationBranches",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationBranches_LastModifierUserId",
                table: "NotificationBranches",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationBranches_NotificationId",
                table: "NotificationBranches",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatorUserId",
                table: "Notifications",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_LastModifierUserId",
                table: "Notifications",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUsers_NotificationId",
                table: "NotificationUsers",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUsers_UserId",
                table: "NotificationUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_CreatorUserId",
                table: "OrderDetails",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_LastModifierUserId",
                table: "OrderDetails",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_QrCode",
                table: "OrderDetails",
                column: "QrCode");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_SpoonCode",
                table: "OrderDetails",
                column: "SpoonCode",
                unique: true,
                filter: "[SpoonCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BranchId",
                table: "Orders",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatorUserId",
                table: "Orders",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_LastModifierUserId",
                table: "Orders",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                table: "Orders",
                column: "OrderNumber",
                unique: true,
                filter: "[OrderNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TicketInvestmentId",
                table: "Orders",
                column: "TicketInvestmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ConsumerPhone_TotalAvailablePoints",
                table: "Orders",
                columns: new[] { "ConsumerPhone", "TotalAvailablePoints" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderDate_TotalAvailablePoints",
                table: "Orders",
                columns: new[] { "OrderDate", "TotalAvailablePoints" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderTickets_CreatorUserId",
                table: "OrderTickets",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTickets_LastModifierUserId",
                table: "OrderTickets",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTickets_OrderId",
                table: "OrderTickets",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTickets_QrCode",
                table: "OrderTickets",
                column: "QrCode");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTickets_TicketId",
                table: "OrderTickets",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Code",
                table: "Permissions",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_CreatorUserId",
                table: "Permissions",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_LastModifierUserId",
                table: "Permissions",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductClasses_Code",
                table: "ProductClasses",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductClasses_CreatorUserId",
                table: "ProductClasses",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductClasses_LastModifierUserId",
                table: "ProductClasses",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductClasses_RewardCode",
                table: "ProductClasses",
                column: "RewardCode");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPoints_CreatorUserId",
                table: "ProductPoints",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPoints_LastModifierUserId",
                table: "ProductPoints",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPoints_ProductId",
                table: "ProductPoints",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_CreatorUserId",
                table: "ProductPrices",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_CustomerId",
                table: "ProductPrices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_LastModifierUserId",
                table: "ProductPrices",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_ProductId",
                table: "ProductPrices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_SalesOrgId",
                table: "ProductPrices",
                column: "SalesOrgId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatorUserId",
                table: "Products",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_LastModifierUserId",
                table: "Products",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductClassId",
                table: "Products",
                column: "ProductClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubProductClassId",
                table: "Products",
                column: "SubProductClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUnits_CreatorUserId",
                table: "ProductUnits",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUnits_LastModifierUserId",
                table: "ProductUnits",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_Code",
                table: "Provinces",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_CreatorUserId",
                table: "Provinces",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_LastModifierUserId",
                table: "Provinces",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardBranches_BranchId",
                table: "RewardBranches",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardBranches_CreatorUserId",
                table: "RewardBranches",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardBranches_LastModifierUserId",
                table: "RewardBranches",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardBranches_RewardPackageId",
                table: "RewardBranches",
                column: "RewardPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardItems_CreatorUserId",
                table: "RewardItems",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardItems_LastModifierUserId",
                table: "RewardItems",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardItems_ProductId",
                table: "RewardItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardItems_ProductUnitId",
                table: "RewardItems",
                column: "ProductUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardItems_RewardPackageId",
                table: "RewardItems",
                column: "RewardPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardPackages_CreatorUserId",
                table: "RewardPackages",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardPackages_LastModifierUserId",
                table: "RewardPackages",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_CreatorUserId",
                table: "RolePermissions",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_LastModifierUserId",
                table: "RolePermissions",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatorUserId",
                table: "Roles",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_LastModifierUserId",
                table: "Roles",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleName",
                table: "Roles",
                column: "RoleName",
                unique: true,
                filter: "[RoleName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrgs_Code",
                table: "SalesOrgs",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrgs_CreatorUserId",
                table: "SalesOrgs",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrgs_LastModifierUserId",
                table: "SalesOrgs",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrgs_ParentId",
                table: "SalesOrgs",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_AreaId",
                table: "Staffs",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_Code",
                table: "Staffs",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_CreatorUserId",
                table: "Staffs",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_LastModifierUserId",
                table: "Staffs",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_SalesOrgId",
                table: "Staffs",
                column: "SalesOrgId");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_UserId",
                table: "Staffs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_ZoneId",
                table: "Staffs",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_SubProductClasses_Code",
                table: "SubProductClasses",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubProductClasses_CreatorUserId",
                table: "SubProductClasses",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubProductClasses_LastModifierUserId",
                table: "SubProductClasses",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAcceptances_CreatorUserId",
                table: "TicketAcceptances",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAcceptances_LastModifierUserId",
                table: "TicketAcceptances",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAcceptances_TicketInvestmentId",
                table: "TicketAcceptances",
                column: "TicketInvestmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketAcceptances_UpdateUserId",
                table: "TicketAcceptances",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketConsumerRewardDetails_CreatorUserId",
                table: "TicketConsumerRewardDetails",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketConsumerRewardDetails_LastModifierUserId",
                table: "TicketConsumerRewardDetails",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketConsumerRewardDetails_TicketConsumerRewardId",
                table: "TicketConsumerRewardDetails",
                column: "TicketConsumerRewardId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketConsumerRewardDetails_TicketId",
                table: "TicketConsumerRewardDetails",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketConsumerRewards_CreatorUserId",
                table: "TicketConsumerRewards",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketConsumerRewards_LastModifierUserId",
                table: "TicketConsumerRewards",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketConsumerRewards_RewardItemId",
                table: "TicketConsumerRewards",
                column: "RewardItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketConsumerRewards_TicketInvestmentId",
                table: "TicketConsumerRewards",
                column: "TicketInvestmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketFinalSettlements_CreatorUserId",
                table: "TicketFinalSettlements",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketFinalSettlements_DecideUserId",
                table: "TicketFinalSettlements",
                column: "DecideUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketFinalSettlements_LastModifierUserId",
                table: "TicketFinalSettlements",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketFinalSettlements_TicketInvestmentId",
                table: "TicketFinalSettlements",
                column: "TicketInvestmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketFinalSettlements_UpdateUserId",
                table: "TicketFinalSettlements",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInvestmentHistories_CreatorUserId",
                table: "TicketInvestmentHistories",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInvestmentHistories_TicketInvestmentId",
                table: "TicketInvestmentHistories",
                column: "TicketInvestmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInvestments_BudgetId",
                table: "TicketInvestments",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInvestments_Code",
                table: "TicketInvestments",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInvestments_CreatorUserId",
                table: "TicketInvestments",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInvestments_CustomerId",
                table: "TicketInvestments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInvestments_CycleId",
                table: "TicketInvestments",
                column: "CycleId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInvestments_LastModifierUserId",
                table: "TicketInvestments",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInvestments_RegisterStaffId",
                table: "TicketInvestments",
                column: "RegisterStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInvestments_RewardPackageId",
                table: "TicketInvestments",
                column: "RewardPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInvestments_Status_CreationTime",
                table: "TicketInvestments",
                columns: new[] { "Status", "CreationTime" });

            migrationBuilder.CreateIndex(
                name: "IX_TicketMaterials_CreatorUserId",
                table: "TicketMaterials",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMaterials_LastModifierUserId",
                table: "TicketMaterials",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMaterials_MaterialId",
                table: "TicketMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMaterials_TicketInvestmentId",
                table: "TicketMaterials",
                column: "TicketInvestmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketOperations_CreatorUserId",
                table: "TicketOperations",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketOperations_LastModifierUserId",
                table: "TicketOperations",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketOperations_TicketInvestmentId",
                table: "TicketOperations",
                column: "TicketInvestmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketOperations_UpdateUserId",
                table: "TicketOperations",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketProgresses_CreatorUserId",
                table: "TicketProgresses",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketProgresses_LastModifierUserId",
                table: "TicketProgresses",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketProgresses_TicketInvestmentId",
                table: "TicketProgresses",
                column: "TicketInvestmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketProgresses_UpdateUserId",
                table: "TicketProgresses",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketProgressMaterials_CreatorUserId",
                table: "TicketProgressMaterials",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketProgressMaterials_LastModifierUserId",
                table: "TicketProgressMaterials",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketProgressMaterials_MaterialId",
                table: "TicketProgressMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketProgressMaterials_TicketProgressId",
                table: "TicketProgressMaterials",
                column: "TicketProgressId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketProgressRewardItems_CreatorUserId",
                table: "TicketProgressRewardItems",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketProgressRewardItems_LastModifierUserId",
                table: "TicketProgressRewardItems",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketProgressRewardItems_RewardItemId",
                table: "TicketProgressRewardItems",
                column: "RewardItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketProgressRewardItems_TicketProgressId",
                table: "TicketProgressRewardItems",
                column: "TicketProgressId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketRewardItems_CreatorUserId",
                table: "TicketRewardItems",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketRewardItems_LastModifierUserId",
                table: "TicketRewardItems",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketRewardItems_RewardItemId",
                table: "TicketRewardItems",
                column: "RewardItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketRewardItems_TicketInvestmentId",
                table: "TicketRewardItems",
                column: "TicketInvestmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_LastPrintUserId",
                table: "Tickets",
                column: "LastPrintUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketInvestmentId",
                table: "Tickets",
                column: "TicketInvestmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSalesCommitments_CreatorUserId",
                table: "TicketSalesCommitments",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSalesCommitments_LastModifierUserId",
                table: "TicketSalesCommitments",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSalesCommitments_TicketInvestmentId",
                table: "TicketSalesCommitments",
                column: "TicketInvestmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssignments_CreatorUserId",
                table: "UserAssignments",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssignments_LastModifierUserId",
                table: "UserAssignments",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssignments_SalesOrgId",
                table: "UserAssignments",
                column: "SalesOrgId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssignments_UserId_SalesOrgId",
                table: "UserAssignments",
                columns: new[] { "UserId", "SalesOrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserResetTickets_Token",
                table: "UserResetTickets",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserResetTickets_UserId",
                table: "UserResetTickets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_CreatorUserId",
                table: "UserRoles",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_LastModifierUserId",
                table: "UserRoles",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatorUserId",
                table: "Users",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LastModifierUserId",
                table: "Users",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_Code",
                table: "Wards",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wards_CreatorUserId",
                table: "Wards",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_DistrictId",
                table: "Wards",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_LastModifierUserId",
                table: "Wards",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Zones_Code",
                table: "Zones",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Zones_CreatorUserId",
                table: "Zones",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Zones_LastModifierUserId",
                table: "Zones",
                column: "LastModifierUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppLogs");

            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropTable(
                name: "BudgetDetails");

            migrationBuilder.DropTable(
                name: "Consumers");

            migrationBuilder.DropTable(
                name: "CustomerSales");

            migrationBuilder.DropTable(
                name: "CustomerSalesItems");

            migrationBuilder.DropTable(
                name: "InvestmentBranchSettings");

            migrationBuilder.DropTable(
                name: "InvestmentSetting");

            migrationBuilder.DropTable(
                name: "NotificationBranches");

            migrationBuilder.DropTable(
                name: "NotificationUsers");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "OrderTickets");

            migrationBuilder.DropTable(
                name: "ProductPoints");

            migrationBuilder.DropTable(
                name: "ProductPrices");

            migrationBuilder.DropTable(
                name: "RewardBranches");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "TicketAcceptances");

            migrationBuilder.DropTable(
                name: "TicketConsumerRewardDetails");

            migrationBuilder.DropTable(
                name: "TicketFinalSettlements");

            migrationBuilder.DropTable(
                name: "TicketInvestmentHistories");

            migrationBuilder.DropTable(
                name: "TicketMaterials");

            migrationBuilder.DropTable(
                name: "TicketOperations");

            migrationBuilder.DropTable(
                name: "TicketProgressMaterials");

            migrationBuilder.DropTable(
                name: "TicketProgressRewardItems");

            migrationBuilder.DropTable(
                name: "TicketRewardItems");

            migrationBuilder.DropTable(
                name: "TicketSalesCommitments");

            migrationBuilder.DropTable(
                name: "UserAssignments");

            migrationBuilder.DropTable(
                name: "UserResetTickets");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "TicketConsumerRewards");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "TicketProgresses");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "RewardItems");

            migrationBuilder.DropTable(
                name: "MaterialTypes");

            migrationBuilder.DropTable(
                name: "TicketInvestments");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductUnits");

            migrationBuilder.DropTable(
                name: "Budgets");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "RewardPackages");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "ProductClasses");

            migrationBuilder.DropTable(
                name: "SubProductClasses");

            migrationBuilder.DropTable(
                name: "Cycles");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Staffs");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "SalesOrgs");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Zones");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropSequence(
                name: "app_setting_seq");

            migrationBuilder.DropSequence(
                name: "area_seq");

            migrationBuilder.DropSequence(
                name: "branch_seq");

            migrationBuilder.DropSequence(
                name: "brand_seq");

            migrationBuilder.DropSequence(
                name: "budget_detail_seq");

            migrationBuilder.DropSequence(
                name: "budget_seq");

            migrationBuilder.DropSequence(
                name: "channel_seq");

            migrationBuilder.DropSequence(
                name: "consumer_seq");

            migrationBuilder.DropSequence(
                name: "customer_sale_seq");

            migrationBuilder.DropSequence(
                name: "customer_sales_item_seq");

            migrationBuilder.DropSequence(
                name: "customer_seq");

            migrationBuilder.DropSequence(
                name: "cycle_seq");

            migrationBuilder.DropSequence(
                name: "district_seq");

            migrationBuilder.DropSequence(
                name: "investment_branch_setting_seq");

            migrationBuilder.DropSequence(
                name: "investmentSetting_seq");

            migrationBuilder.DropSequence(
                name: "material_seq");

            migrationBuilder.DropSequence(
                name: "material_type_seq");

            migrationBuilder.DropSequence(
                name: "notification_branch_seq");

            migrationBuilder.DropSequence(
                name: "notification_seq");

            migrationBuilder.DropSequence(
                name: "notification_user_seq");

            migrationBuilder.DropSequence(
                name: "order_detail_seq");

            migrationBuilder.DropSequence(
                name: "order_seq");

            migrationBuilder.DropSequence(
                name: "order_ticket_seq");

            migrationBuilder.DropSequence(
                name: "permission_seq");

            migrationBuilder.DropSequence(
                name: "product_class_seq");

            migrationBuilder.DropSequence(
                name: "product_point_seq");

            migrationBuilder.DropSequence(
                name: "product_seq");

            migrationBuilder.DropSequence(
                name: "product_unit_seq");

            migrationBuilder.DropSequence(
                name: "province_seq");

            migrationBuilder.DropSequence(
                name: "reward_branch_seq");

            migrationBuilder.DropSequence(
                name: "reward_item_seq");

            migrationBuilder.DropSequence(
                name: "reward_package_seq");

            migrationBuilder.DropSequence(
                name: "role_permission_seq");

            migrationBuilder.DropSequence(
                name: "role_seq");

            migrationBuilder.DropSequence(
                name: "staff_seq");

            migrationBuilder.DropSequence(
                name: "sub_product_class_seq");

            migrationBuilder.DropSequence(
                name: "ticket_acceptance_seq");

            migrationBuilder.DropSequence(
                name: "ticket_customer_reward_detail_seq");

            migrationBuilder.DropSequence(
                name: "ticket_customer_reward_seq");

            migrationBuilder.DropSequence(
                name: "ticket_final_settlement_seq");

            migrationBuilder.DropSequence(
                name: "ticket_investment_history_seq");

            migrationBuilder.DropSequence(
                name: "ticket_investment_seq");

            migrationBuilder.DropSequence(
                name: "ticket_material_seq");

            migrationBuilder.DropSequence(
                name: "ticket_operation_seq");

            migrationBuilder.DropSequence(
                name: "ticket_progress_material_seq");

            migrationBuilder.DropSequence(
                name: "ticket_progress_reward_item_seq");

            migrationBuilder.DropSequence(
                name: "ticket_progress_seq");

            migrationBuilder.DropSequence(
                name: "ticket_reward_item_seq");

            migrationBuilder.DropSequence(
                name: "ticket_sales_commitment_seq");

            migrationBuilder.DropSequence(
                name: "ticket_seq");

            migrationBuilder.DropSequence(
                name: "user_assignment_seq");

            migrationBuilder.DropSequence(
                name: "user_reset_ticket_seq");

            migrationBuilder.DropSequence(
                name: "user_role_seq");

            migrationBuilder.DropSequence(
                name: "user_seq");

            migrationBuilder.DropSequence(
                name: "ward_seq");

            migrationBuilder.DropSequence(
                name: "zone_seq");
        }
    }
}
