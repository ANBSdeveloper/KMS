using Cbms.EntityFrameworkCore;
using Cbms.Kms.Domain.AppLogs;
using Cbms.Kms.Domain.AppSettings;
using Cbms.Kms.Domain.Areas;
using Cbms.Kms.Domain.Branches;
using Cbms.Kms.Domain.Brands;
using Cbms.Kms.Domain.Budgets;
using Cbms.Kms.Domain.Channels;
using Cbms.Kms.Domain.Consumers;
using Cbms.Kms.Domain.CustomerLocations;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.CustomerSales;
using Cbms.Kms.Domain.CustomerSalesItems;
using Cbms.Kms.Domain.Cycles;
using Cbms.Kms.Domain.Geography.Districts;
using Cbms.Kms.Domain.Geography.Provinces;
using Cbms.Kms.Domain.Geography.Wards;
using Cbms.Kms.Domain.InvestmentBranchSettings;
using Cbms.Kms.Domain.InvestmentSettings;
using Cbms.Kms.Domain.Materials;
using Cbms.Kms.Domain.MaterialTypes;
using Cbms.Kms.Domain.Notifications;
using Cbms.Kms.Domain.Orders;
using Cbms.Kms.Domain.PosmClasses;
using Cbms.Kms.Domain.PosmInvestments;
using Cbms.Kms.Domain.PosmItems;
using Cbms.Kms.Domain.PosmPrices;
using Cbms.Kms.Domain.PosmTypes;
using Cbms.Kms.Domain.ProductClasses;
using Cbms.Kms.Domain.ProductPoints;
using Cbms.Kms.Domain.ProductPrices;
using Cbms.Kms.Domain.Products;
using Cbms.Kms.Domain.ProductUnits;
using Cbms.Kms.Domain.RewardPackages;
using Cbms.Kms.Domain.SalesOrgs;
using Cbms.Kms.Domain.Staffs;
using Cbms.Kms.Domain.SubProductClasses;
using Cbms.Kms.Domain.TicketInvestments;
using Cbms.Kms.Domain.Users;
using Cbms.Kms.Domain.Vendors;
using Cbms.Kms.Domain.Zones;
using Cbms.Kms.Infrastructure.EntityFramework.Brands;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.AppLogs;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.AppSettings;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.Areas;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.Branches;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.Budgets;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.Channels;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.Consumers;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.CustomerLocations;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.Customers;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.CustomerSales;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.CustomerSalesItems;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.Geography;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.Infrastructure;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.Materials;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.MaterialTypes;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.Notifications;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.Orders;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.PosmClasses;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.PosmInvestments;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.PosmItems;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.PosmTypes;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.RewardPackages;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.SalesOrgs;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.Staffs;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.TicketInvestments;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.Vendors;
using Cbms.Kms.Infrastructure.EntityFramework.Configuration.Zones;
using Cbms.Kms.Infrastructure.EntityFramework.Cycles;
using Cbms.Kms.Infrastructure.EntityFramework.Districts;
using Cbms.Kms.Infrastructure.EntityFramework.Geography;
using Cbms.Kms.Infrastructure.EntityFramework.ProductClasses;
using Cbms.Kms.Infrastructure.EntityFramework.ProductPoints;
using Cbms.Kms.Infrastructure.EntityFramework.ProductPrices;
using Cbms.Kms.Infrastructure.EntityFramework.Products;
using Cbms.Kms.Infrastructure.EntityFramework.ProductUnits;
using Cbms.Kms.Infrastructure.EntityFramework.SubProductClasses;
using Cbms.Kms.Infrastructure.EntityFramework.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Cbms.Kms.Infrastructure
{
    public class AppDbContext : AppDbContextBase
    {
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductClass> ProductClasses { get; set; }
        public DbSet<SubProductClass> SubProductClasses { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Cycle> Cycles { get; set; }
        public DbSet<SalesOrg> SalesOrgs { get; set; }
        public DbSet<UserAssignment> UserAssignments { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<CustomerSale> CustomerSales { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetZone> BudgetZones { get; set; }
        public DbSet<BudgetArea> BudgetAreas { get; set; }
        public DbSet<BudgetBranch> BudgetBranches { get; set; }
        public DbSet<RewardBranch> RewardBranches { get; set; }
        public DbSet<RewardItem> RewardItems { get; set; }
        public DbSet<RewardPackage> RewardPackages { get; set; }
        public DbSet<InvestmentSetting> InvestmentSetting { get; set; }
        public DbSet<ProductUnit> ProductUnits { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<TicketInvestment> TicketInvestments { get; set; }
        public DbSet<TicketInvestmentHistory> TicketInvestmentHistories { get; set; }
        public DbSet<TicketRewardItem> TicketRewardItems { get; set; }
        public DbSet<TicketMaterial> TicketMaterials { get; set; }
        public DbSet<TicketSalesCommitment> TicketSalesCommitments { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketProgress> TicketUpdates { get; set; }
        public DbSet<TicketProgressRewardItem> TicketProgressRewardItems { get; set; }
        public DbSet<TicketProgressMaterial> TicketProgressMaterials { get; set; }
        public DbSet<TicketOperation> TicketOperations { get; set; }
        public DbSet<TicketAcceptance> TicketAcceptances { get; set; }
        public DbSet<TicketConsumerReward> TicketConsumerRewards { get; set; }
        public DbSet<TicketConsumerRewardDetail> TicketConsumerRewardDetails { get; set; }
        public DbSet<TicketFinalSettlement> TicketFinalSettlements { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationBranch> NotificationBranches { get; set; }
        public DbSet<NotificationUser> NotificationUsers { get; set; }
        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderTicket> OrderTickets { get; set; }
        public DbSet<AppLog> AppLogs { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
        public DbSet<ProductPoint> ProductPoints { get; set; }
        public DbSet<ProductPointHistory> ProductPointHistories { get; set; }
        public DbSet<MaterialType> MaterialTypes { get; set; }
        public DbSet<InvestmentBranchSetting> InvestmentBranchSettings { get; set; }
        public DbSet<CustomerSalesItem> CustomerSalesItems { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<PosmClass> PosmClasses { get; set; }
        public DbSet<PosmType> PosmTypes { get; set; }
        public DbSet<PosmItem> PosmItems { get; set; }
        public DbSet<PosmCatalog> PosmCatalogs { get; set; }
        public DbSet<PosmPriceHeader> PosmPriceeHeaders { get; set; }
        public DbSet<PosmPriceDetail> PosmPriceDetails { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<CustomerLocation> CustomerLocations { get; set; }
        public DbSet<PosmInvestment> PosmInvestments { get; set; }
        public DbSet<PosmInvestmentItem> PosmInvestmentItems { get; set; }
        public DbSet<PosmSalesCommitment> PosmSalesCommitments { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BrandEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new ProductClassEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new SubProductClassEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new ProvinceEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new DistrictEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new ZoneEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new BranchEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new AreaEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new CycleEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new WardEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new SalesOrgEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new UserAssignmentEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new CustomerSaleEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new StaffEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new BudgetEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new BudgetZoneEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new BudgetAreaEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new BudgetBranchEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new RewardBranchEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new RewardItemEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new RewardPackageEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new InvestmentSettingEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new ProductUnitEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new MaterialEntityTypeConfiguration(modelBuilder));

            modelBuilder.ApplyConfiguration(new TicketInvestmentEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new TicketInvestmentHistoryEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new TicketRewardItemEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new TicketMaterialEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new TicketEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new TicketProgressEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new TicketProgressRewardItemEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new TicketProgressMaterialEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new TicketConsumerRewardEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new TicketConsumerRewardDetailEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new TicketSalesCommitmentEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new TicketFinalSettlementEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new TicketAcceptanceEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new TicketOperationEntityTypeConfiguration(modelBuilder));

            modelBuilder.ApplyConfiguration(new NotificationEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new NotificationBranchEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new NotificationUserEntityTypeConfiguration(modelBuilder));

            modelBuilder.ApplyConfiguration(new ConsumerEntityTypeConfiguration(modelBuilder));

            modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new OrderDetailEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new OrderTicketEntityTypeConfiguration(modelBuilder));

            modelBuilder.ApplyConfiguration(new AppLogEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new AppSettingEntityTypeConfiguration(modelBuilder));

            modelBuilder.ApplyConfiguration(new ProductPriceEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new ProductPointEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new ProductPointHistoryEntityTypeConfiguration(modelBuilder));


            modelBuilder.ApplyConfiguration(new MaterialTypeEntityTypeConfiguration(modelBuilder));

            modelBuilder.ApplyConfiguration(new InvestmentBranchSettingEntityTypeConfiguration(modelBuilder));

            modelBuilder.ApplyConfiguration(new CustomerSalesItemEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new ChannelEntityTypeConfiguration(modelBuilder));

            modelBuilder.ApplyConfiguration(new PosmClassEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new PosmTypeEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new PosmItemEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new PosmCatalogEntityTypeConfiguration(modelBuilder));

            modelBuilder.ApplyConfiguration(new PosmPriceHeaderEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new PosmPriceDetailEntityTypeConfiguration(modelBuilder));

            modelBuilder.ApplyConfiguration(new VendorEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new CustomerLocationEntityTypeConfiguration(modelBuilder));

            modelBuilder.ApplyConfiguration(new PosmInvestmentEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new PosmInvestmentItemEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new PosmSalesCommitmentEntityTypeConfiguration(modelBuilder));
            modelBuilder.ApplyConfiguration(new PosmInvestmentItemHistoryEntityTypeConfiguration(modelBuilder));
        }
    }

    public class HostDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .Build();

            var connectionString = configuration["ConnectionString"];

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlServer(connectionString, (options) =>
            {
                options.MigrationsAssembly(GetType().Assembly.GetName().Name);
                options.MigrationsHistoryTable("_MigrationsHistory");
            });

            return new AppDbContext(builder.Options);
        }
    }
}