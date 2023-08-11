using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Materials;
using Cbms.Kms.Domain.TicketInvestments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.TicketInvestments
{
    internal class TicketProgressMaterialEntityTypeConfiguration : IEntityTypeConfiguration<TicketProgressMaterial>
    {
        private readonly ModelBuilder _modelBuilder;

        public TicketProgressMaterialEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<TicketProgressMaterial> builder)
        {
            _modelBuilder.HasSequence("ticket_progress_material_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("TicketProgressMaterials");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("ticket_progress_material_seq");

            builder.HasOne<Material>().WithMany().HasForeignKey(p => p.MaterialId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}