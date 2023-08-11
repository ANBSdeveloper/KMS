using Cbms.Authorization.Users;
using Cbms.Kms.Domain.MaterialTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.MaterialTypes
{
    internal class MaterialTypeEntityTypeConfiguration : IEntityTypeConfiguration<MaterialType>
    {
        private readonly ModelBuilder _modelBuilder;

        public MaterialTypeEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<MaterialType> builder)
        {
            _modelBuilder.HasSequence("material_type_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("MaterialTypes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("material_type_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true);

            builder.Property(x => x.Name).HasMaxLength(200).IsUnicode(true).IsRequired(true);

            builder.HasIndex(x => x.Code).IsUnique();

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
