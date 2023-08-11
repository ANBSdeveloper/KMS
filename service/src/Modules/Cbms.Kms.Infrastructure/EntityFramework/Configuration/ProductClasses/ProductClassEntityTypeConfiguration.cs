using Cbms.Authorization.Users;
using Cbms.Kms.Domain.ProductClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.ProductClasses
{
    internal class ProductClassEntityTypeConfiguration : IEntityTypeConfiguration<ProductClass>
    {
        private readonly ModelBuilder _modelBuilder;

        public ProductClassEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<ProductClass> builder)
        {
            _modelBuilder.HasSequence("product_class_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("ProductClasses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("product_class_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true);

            builder.Property(x => x.Name).HasMaxLength(200).IsUnicode(true).IsRequired(true);

            builder.Property(x => x.RewardCode).HasMaxLength(50).IsRequired(true).HasDefaultValue("");

            builder.HasIndex(x => x.Code).IsUnique();
            builder.HasIndex(x => x.RewardCode);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}