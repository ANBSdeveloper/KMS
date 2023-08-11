using Cbms.Authorization.Users;
using Cbms.Kms.Domain.SalesOrgs;
using Cbms.Kms.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Users
{
    internal class UserAssignmentEntityTypeConfiguration : IEntityTypeConfiguration<UserAssignment>
    {
        private readonly ModelBuilder _modelBuilder;

        public UserAssignmentEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<UserAssignment> builder)
        {
            _modelBuilder.HasSequence("user_assignment_seq").IncrementsBy(10).StartsAt(1);

            builder.ToTable("UserAssignments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("user_assignment_seq");

            builder.HasIndex(c => new { c.UserId, c.SalesOrgId });

            builder.HasOne<SalesOrg>().WithMany().HasForeignKey(p => p.SalesOrgId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.UserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}