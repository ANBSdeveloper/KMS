using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Cycles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Cycles
{
    internal class CycleEntityTypeConfiguration : IEntityTypeConfiguration<Cycle>
    {
        private readonly ModelBuilder _modelBuilder;

        public CycleEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<Cycle> builder)
        {
            _modelBuilder.HasSequence("cycle_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("Cycles");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("cycle_seq");

            builder.Property(x => x.Number).HasMaxLength(10).IsRequired(true).IsUnicode(false);

            builder.HasIndex(x => x.Number).IsUnique();

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}