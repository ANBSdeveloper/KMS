using Cbms.Authorization.Users;
using Cbms.Kms.Domain.PosmTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.PosmTypes
{
    internal class PosmTypeEntityTypeConfiguration : IEntityTypeConfiguration<PosmType>
    {
        private readonly ModelBuilder _modelBuilder;

        public PosmTypeEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<PosmType> builder)
        {
            _modelBuilder.HasSequence("posm_type_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("PosmTypes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("posm_type_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true);

            builder.Property(x => x.Name).HasMaxLength(200).IsUnicode(true).IsRequired(true);

            builder.HasIndex(x => x.Code).IsUnique();

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
