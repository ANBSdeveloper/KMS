using Cbms.Authorization.Users;
using Cbms.Kms.Domain.AppSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.AppSettings
{
    internal class AppSettingEntityTypeConfiguration : IEntityTypeConfiguration<AppSetting>
    {
        private readonly ModelBuilder _modelBuilder;

        public AppSettingEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<AppSetting> builder)
        {
            _modelBuilder.HasSequence("app_setting_seq").IncrementsBy(1).StartsAt(1);

            builder.ToTable("AppSettings");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("app_setting_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(true).HasConversion(v => v.ToUpperInvariant(), v => v); ;
            builder.Property(x => x.Description).HasMaxLength(500);

            builder.HasIndex(x => x.Code);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}