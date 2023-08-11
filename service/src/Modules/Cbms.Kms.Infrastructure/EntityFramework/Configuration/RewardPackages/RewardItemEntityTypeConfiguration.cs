using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Products;
using Cbms.Kms.Domain.ProductUnits;
using Cbms.Kms.Domain.RewardPackages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.RewardPackages
{
    internal class RewardItemEntityTypeConfiguration : IEntityTypeConfiguration<RewardItem>
    {
        private readonly ModelBuilder _modelBuilder;

        public RewardItemEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<RewardItem> builder)
        {
            _modelBuilder.HasSequence("reward_item_seq").IncrementsBy(10).StartsAt(1);

            builder.ToTable("RewardItems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("reward_item_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true).IsUnicode(false);
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired(true).IsUnicode(true);
            builder.Property(x => x.DocumentLink).HasMaxLength(1000).IsUnicode(true);
            builder.Property(x => x.Price).HasColumnType("decimal(18,0)");

            builder.Property(x => x.ProductId).IsRequired(false);
            builder.Property(x => x.ProductUnitId).IsRequired(false);

            builder.HasIndex(p => new { p.RewardPackageId, p.Code }).IsUnique();

            builder.HasOne<Product>().WithMany().HasForeignKey(p => p.ProductId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<ProductUnit>().WithMany().HasForeignKey(p => p.ProductUnitId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}