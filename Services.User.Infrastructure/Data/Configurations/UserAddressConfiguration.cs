using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.User.Domain.Entities;

namespace Services.User.Infrastructure.Data.Configurations
{
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.ToTable("UserAddresses");

            builder.HasKey(ua => ua.UserId);

            builder.Property(ua => ua.UserId)
                .IsRequired();

            builder.Property(ua => ua.Address)
                .HasMaxLength(500);

            builder.Property(ua => ua.Street)
                .HasMaxLength(200);

            builder.Property(ua => ua.City)
                .HasMaxLength(100);

            builder.Property(ua => ua.State)
                .HasMaxLength(100);

            builder.Property(ua => ua.PostalCode)
                .HasMaxLength(20);

            builder.Property(ua => ua.Country)
                .HasMaxLength(100);

            builder.Property(ua => ua.IsDefault)
                .IsRequired();

            builder.HasOne<Domain.Entities.User>()
                .WithMany(u => u.Addresses)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}