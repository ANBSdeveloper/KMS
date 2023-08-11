using Cbms.Authorization.Users;
using Cbms.Kms.Domain.PosmItems;
using Cbms.Kms.Domain.PosmPrices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.PosmItems
{
    internal class PosmPriceDetailEntityTypeConfiguration : IEntityTypeConfiguration<PosmPriceDetail>
    {
        private readonly ModelBuilder _modelBuilder;

        public PosmPriceDetailEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<PosmPriceDetail> builder)
        {
            _modelBuilder.HasSequence("posm_price_detail").IncrementsBy(10).StartsAt(1);

            builder.ToTable("PosmPriceDetails");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("posm_price_detail");

            builder.Property(x => x.Price).HasColumnType("DECIMAL(18,2)");
         
         

            builder.HasOne<PosmItem>().WithMany().HasForeignKey(p => p.PosmItemId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}