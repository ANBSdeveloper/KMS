using Cbms.Authorization.Users;
using Cbms.Kms.Domain.AppLogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.AppLogs
{
    internal class AppLogEntityTypeConfiguration : IEntityTypeConfiguration<AppLog>
    {
        private readonly ModelBuilder _modelBuilder;

        public AppLogEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<AppLog> builder)
        {
            builder.ToTable("AppLogs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(50).IsRequired(true);

            builder.HasIndex(x => x.Name);

        }
    }
}