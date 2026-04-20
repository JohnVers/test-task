using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using task.Models;

namespace task.Data.Configurations;

public class OfficeConfiguration : IEntityTypeConfiguration<Office>
{
    public void Configure(EntityTypeBuilder<Office> builder)
    {
        builder.ToTable("offices");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).UseIdentityColumn();

        builder.Property(o => o.Code).HasMaxLength(100);
        builder.Property(o => o.Uuid).HasMaxLength(50);
        builder.Property(o => o.CountryCode).HasMaxLength(10).IsRequired();
        builder.Property(o => o.AddressCity).HasMaxLength(200);
        builder.Property(o => o.AddressRegion).HasMaxLength(200);
        builder.Property(o => o.AddressStreet).HasMaxLength(500);
        builder.Property(o => o.AddressHouseNumber).HasMaxLength(50);
        builder.Property(o => o.WorkTime).HasMaxLength(500);

        builder.OwnsOne(o => o.Coordinates, coords =>
        {
            coords.Property(c => c.Latitude).HasColumnName("latitude");
            coords.Property(c => c.Longitude).HasColumnName("longitude");
        });

        builder.HasOne(o => o.Phones)
            .WithOne(p => p.Office)
            .HasForeignKey<Phone>(p => p.OfficeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(o => o.AddressCity).HasDatabaseName("ix_offices_address_city");
        builder.HasIndex(o => o.AddressRegion).HasDatabaseName("ix_offices_address_region");
        builder.HasIndex(o => o.CityCode).HasDatabaseName("ix_offices_city_code");
    }
}
