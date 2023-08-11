using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Brands;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Brands
{
    internal class BrandEntityTypeConfiguration : IEntityTypeConfiguration<Brand>
    {
        private readonly ModelBuilder _modelBuilder;

        public BrandEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            _modelBuilder.HasSequence("brand_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("Brands");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("brand_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true);

            builder.Property(x => x.Name).HasMaxLength(200).IsUnicode(true).IsRequired(true);

            builder.HasIndex(x => x.Code).IsUnique();

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}