using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Areas;
using Cbms.Kms.Domain.Branches;
using Cbms.Kms.Domain.Channels;
using Cbms.Kms.Domain.SalesOrgs;
using Cbms.Kms.Domain.Zones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Branches
{
    internal class BranchEntityTypeConfiguration : IEntityTypeConfiguration<Branch>
    {
        private readonly ModelBuilder _modelBuilder;

        public BranchEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            _modelBuilder.HasSequence("branch_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("Branches");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("branch_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true);

            builder.Property(x => x.Name).HasMaxLength(200).IsUnicode(true).IsRequired(true);
            builder.Property(x => x.AreaId).IsRequired(false);
            builder.Property(x => x.ZoneId).IsRequired(false);
            builder.Property(x => x.ChannelId).IsRequired(false);

            builder.HasIndex(x => x.Code).IsUnique();

            builder.HasOne<Channel>().WithMany().HasForeignKey(p => p.ChannelId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Zone>().WithMany().HasForeignKey(p => p.ZoneId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Area>().WithMany().HasForeignKey(p => p.AreaId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<SalesOrg>().WithMany().HasForeignKey(p => p.SalesOrgId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}