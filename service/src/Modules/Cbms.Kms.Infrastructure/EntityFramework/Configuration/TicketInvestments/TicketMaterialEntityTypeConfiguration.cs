using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Materials;
using Cbms.Kms.Domain.TicketInvestments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.TicketInvestments
{
    internal class TicketMaterialEntityTypeConfiguration : IEntityTypeConfiguration<TicketMaterial>
    {
        private readonly ModelBuilder _modelBuilder;

        public TicketMaterialEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<TicketMaterial> builder)
        {
            _modelBuilder.HasSequence("ticket_material_seq").IncrementsBy(5).StartsAt(1);

            builder.ToTable("TicketMaterials");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("ticket_material_seq");
            builder.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Note).HasMaxLength(500).HasDefaultValue("");

            builder.HasOne<Material>().WithMany().HasForeignKey(p => p.MaterialId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}