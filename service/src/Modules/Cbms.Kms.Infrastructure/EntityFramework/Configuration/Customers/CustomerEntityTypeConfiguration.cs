using Cbms.Authorization.Users;
using Cbms.Kms.Domain.Areas;
using Cbms.Kms.Domain.Branches;
using Cbms.Kms.Domain.Customers;
using Cbms.Kms.Domain.Geography.Districts;
using Cbms.Kms.Domain.Geography.Provinces;
using Cbms.Kms.Domain.Geography.Wards;
using Cbms.Kms.Domain.Staffs;
using Cbms.Kms.Domain.Zones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cbms.Kms.Infrastructure.EntityFramework.Configuration.Customers
{
    internal class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        private readonly ModelBuilder _modelBuilder;

        public CustomerEntityTypeConfiguration(ModelBuilder modelBuilder) : base()
        {
            _modelBuilder = modelBuilder;
        }

        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            _modelBuilder.HasSequence("customer_seq").IncrementsBy(100).StartsAt(1);

            builder.ToTable("Customers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("customer_seq");

            builder.Property(x => x.Code).HasMaxLength(50).IsUnicode(false);

            builder.Property(x => x.Name).HasMaxLength(200);
            builder.Property(x => x.ContactName).HasMaxLength(200);
            builder.Property(x => x.MobilePhone).HasMaxLength(100).IsUnicode(false);
            builder.Property(x => x.Phone).HasMaxLength(100).IsUnicode(false);
            builder.Property(x => x.Email).HasMaxLength(100).IsUnicode(false);
            builder.Property(x => x.ChannelCode).HasMaxLength(50).IsUnicode(false);
            builder.Property(x => x.ChannelName).HasMaxLength(200);
            builder.Property(x => x.HouseNumber).HasMaxLength(200);
            builder.Property(x => x.Street).HasMaxLength(100);
            builder.Property(x => x.Address).HasMaxLength(500);

            builder.Property(x => x.IsKeyShop).HasDefaultValue(false);
            builder.Property(x => x.IsAllowKeyShopRegister).HasDefaultValue(true);
            builder.Property(x => x.KeyShopStatus).HasDefaultValue(KeyShopStatus.Unregistered);
            builder.Property(x => x.KeyShopAuthCode).IsUnicode(false).HasMaxLength(10);
            builder.Property(x => x.UserId).IsRequired(false);
            builder.Property(x => x.OtpCode).IsUnicode(false).HasMaxLength(10);
            builder.Property(x => x.OtpTime).IsRequired(false);
            builder.Property(x => x.Efficient).IsRequired(false).HasDefaultValue(null);

            builder.Property(x => x.WardId).IsRequired(false);
            builder.Property(x => x.DistrictId).IsRequired(false);
            builder.Property(x => x.ProvinceId).IsRequired(false);
            builder.Property(x => x.BranchId).IsRequired(false);
            builder.Property(x => x.ZoneId).IsRequired(false);
            builder.Property(x => x.AreaId).IsRequired(false);
            builder.Property(x => x.KeyShopRegisterStaffId).IsRequired(false);

            builder.HasOne<Staff>().WithMany().HasForeignKey(p => p.KeyShopRegisterStaffId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.UserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Zone>().WithMany().HasForeignKey(p => p.ZoneId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Area>().WithMany().HasForeignKey(p => p.AreaId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Ward>().WithMany().HasForeignKey(p => p.WardId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<District>().WithMany().HasForeignKey(p => p.DistrictId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Province>().WithMany().HasForeignKey(p => p.ProvinceId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Branch>().WithMany().HasForeignKey(p => p.BranchId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>().WithMany().HasForeignKey(p => p.CreatorUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().HasForeignKey(p => p.LastModifierUserId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}