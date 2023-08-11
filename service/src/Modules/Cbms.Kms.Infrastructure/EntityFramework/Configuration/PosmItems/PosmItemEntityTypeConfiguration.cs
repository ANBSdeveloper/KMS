using Cbms.Authorization.Users;
using Cbms.Kms.Domain.PosmClasses;
using Cbms.Kms.Domain.PosmItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.PosmItems
{
    internal class PosmItemEntityTypeConfiguration : IEntityTypeConfiguration<PosmItem>
    {
        private readonly ModelBuilder _modelBuilder;

        public PosmItemEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<PosmItem> builder)
        {
            _modelBuilder.HasSequence("posm_item_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("PosmItems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("posm_item_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true).IsUnicode(false);
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired(true).IsUnicode(true);
            builder.Property(x => x.Link).IsRequired(true).IsUnicode(false);
            //builder.Property(x => x.Width).HasColumnType("DECIMAL(18,2)").IsRequired(false);
            //builder.Property(x => x.Height).HasColumnType("DECIMAL(18,2)").IsRequired(false);
            //builder.Property(x => x.SideWidth1).HasColumnType("DECIMAL(18,2)").IsRequired(false);
            //builder.Property(x => x.SideWidth2).HasColumnType("DECIMAL(18,2)").IsRequired(false);
            //builder.Property(x => x.Qty).IsRequired(false);
            //builder.Property(x => x.UnitValue).HasColumnType("DECIMAL(18,2)");

            builder.HasIndex(p => new { p.Code }).IsUnique();

            var catalogNavigation = builder.Metadata.FindNavigation(nameof(PosmItem.PosmCatalogs));
            catalogNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne<PosmClass>().WithMany().HasForeignKey(p => p.PosmClassId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}