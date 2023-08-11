using Cbms.Kms.Domain.ProductPoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.ProductPoints
{
    internal class ProductPointHistoryEntityTypeConfiguration : IEntityTypeConfiguration<ProductPointHistory>
    {
        private readonly ModelBuilder _modelBuilder;

        public ProductPointHistoryEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<ProductPointHistory> builder)
        {
            builder.ToTable("ProductPointHistories");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Points).HasColumnType("decimal(8,2)");
        }
    }
}