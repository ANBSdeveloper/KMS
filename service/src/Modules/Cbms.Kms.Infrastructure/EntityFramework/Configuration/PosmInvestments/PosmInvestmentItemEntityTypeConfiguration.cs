using Cbms.Authorization.Users;
using Cbms.Kms.Domain.PosmClasses;
using Cbms.Kms.Domain.PosmInvestments;
using Cbms.Kms.Domain.PosmItems;
using Cbms.Kms.Domain.Vendors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.TicketInvestments
{
    internal class PosmInvestmentItemEntityTypeConfiguration : IEntityTypeConfiguration<PosmInvestmentItem>
    {
        private readonly ModelBuilder _modelBuilder;

        public PosmInvestmentItemEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<PosmInvestmentItem> builder)
        {
            _modelBuilder.HasSequence("posm_investment_item_seq").IncrementsBy(5).StartsAt(1);

            builder.ToTable("PosmInvestmentItems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("posm_investment_item_seq");
            builder.Property(x => x.PanelShopName).HasMaxLength(500);
            builder.Property(x => x.PanelShopPhone).HasMaxLength(50);
            builder.Property(x => x.PanelShopAddress).HasMaxLength(1000);
            builder.Property(x => x.PanelOtherInfo).HasMaxLength(1000);

            builder.Property(x => x.PanelProductLine).HasMaxLength(1000);
            builder.Property(x => x.Width).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Height).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(x => x.SideWidth1).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(x => x.SideWidth2).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Depth).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Size).IsRequired(false).HasColumnType("decimal(18,2)");
            builder.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Property(x => x.TotalCost).HasColumnType("decimal(18,2)");
            builder.Property(x => x.ActualTotalCost).HasColumnType("decimal(18,2)").IsRequired(false);
            builder.Property(x => x.ActualUnitPrice).HasColumnType("decimal(18,2)").IsRequired(false);
            builder.Property(x => x.PosmValue).HasColumnType("decimal(18,2)");
            builder.Property(x => x.RequestReason).HasMaxLength(1000);
            builder.Property(x => x.RemarkOfCompany).HasColumnType("decimal(10,2)").IsRequired(false);
            builder.Property(x => x.RemarkOfSales).HasColumnType("decimal(10,2)").IsRequired(false);
            builder.Property(x => x.Photo1).IsUnicode(false);
            builder.Property(x => x.Photo2).IsUnicode(false);
            builder.Property(x => x.Photo3).IsUnicode(false);
            builder.Property(x => x.Photo4).IsUnicode(false);

            builder.Property(x => x.OperationPhoto1).IsUnicode(false);
            builder.Property(x => x.OperationPhoto2).IsUnicode(false);
            builder.Property(x => x.OperationPhoto3).IsUnicode(false);
            builder.Property(x => x.OperationPhoto4).IsUnicode(false);
            builder.Property(x => x.OperationLink).IsUnicode(false);
            builder.Property(x => x.OperationNote).HasMaxLength(1000);

            builder.Property(x => x.AcceptancePhoto1).IsUnicode(false);
            builder.Property(x => x.AcceptancePhoto2).IsUnicode(false);
            builder.Property(x => x.AcceptancePhoto3).IsUnicode(false);
            builder.Property(x => x.AcceptancePhoto4).IsUnicode(false);

            builder.Property(x => x.AcceptanceDate).IsRequired(false);
            builder.Property(x => x.AcceptanceNote).HasMaxLength(1000);

            builder.Property(x => x.PrepareNote).IsRequired(false).HasMaxLength(1000);
            builder.Property(x => x.PrepareDate).IsRequired(false);
            builder.Property(x => x.UpdateCostReason).IsUnicode(true).HasMaxLength(1000);

            builder.Property(x => x.VendorId).IsRequired(false);

            builder.HasOne<PosmClass>().WithMany().HasForeignKey(p => p.PosmClassId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<PosmItem>().WithMany().HasForeignKey(p => p.PosmItemId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<PosmCatalog>().WithMany().HasForeignKey(p => p.PosmCatalogId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Vendor>().WithMany().HasForeignKey(p => p.VendorId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}