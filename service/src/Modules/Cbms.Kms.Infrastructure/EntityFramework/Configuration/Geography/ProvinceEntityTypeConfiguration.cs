using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Geography.Provinces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Geography
{
    internal class ProvinceEntityTypeConfiguration : IEntityTypeConfiguration<Province>
    {
        private readonly ModelBuilder _modelBuilder;

        public ProvinceEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<Province> builder)
        {
            _modelBuilder.HasSequence("province_seq").IncrementsBy(10).StartsAt(1);

            builder.ToTable("Provinces");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("province_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true);

            builder.Property(x => x.Name).HasMaxLength(200).IsUnicode(true).IsRequired(true);

            builder.HasIndex(x => x.Code).IsUnique();

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}