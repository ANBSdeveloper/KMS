using Cbms.Authorization.Users;
using Cbms.Kms.Domain.SubProductClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.SubProductClasses
{
    internal class SubProductClassEntityTypeConfiguration : IEntityTypeConfiguration<SubProductClass>
    {
        private readonly ModelBuilder _modelBuilder;

        public SubProductClassEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<SubProductClass> builder)
        {
            _modelBuilder.HasSequence("sub_product_class_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("SubProductClasses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("sub_product_class_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true);

            builder.Property(x => x.Name).HasMaxLength(200).IsUnicode(true).IsRequired(true);

            builder.HasIndex(x => x.Code).IsUnique();

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}