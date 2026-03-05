using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using task.Entities;

namespace task.Data.Configurations;

public class OfficeConfiguration : IEntityTypeConfiguration<Office>
{
    public void Configure(EntityTypeBuilder<Office> builder)
    {
        builder.ToTable("offices");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.Code)
            .HasMaxLength(50);

        builder.Property(e => e.Uuid)
            .HasMaxLength(100);

        builder.Property(e => e.CountryCode)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(e => e.AddressRegion)
            .HasMaxLength(200);

        builder.Property(e => e.AddressCity)
            .HasMaxLength(200);

        builder.Property(e => e.AddressStreet)
            .HasMaxLength(500);

        builder.Property(e => e.AddressHouseNumber)
            .HasMaxLength(50);

        builder.Property(e => e.WorkTime)
            .HasMaxLength(1000);

        builder.OwnsOne(e => e.Coordinates, coords =>
        {
            coords.Property(c => c.Latitude).HasColumnName("latitude");
            coords.Property(c => c.Longitude).HasColumnName("longitude");
        });

        builder.HasMany(e => e.Phones)
            .WithOne(p => p.Office)
            .HasForeignKey(p => p.OfficeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.CityCode);
        builder.HasIndex(e => e.Code);
        builder.HasIndex(e => e.CountryCode);
    }
}
