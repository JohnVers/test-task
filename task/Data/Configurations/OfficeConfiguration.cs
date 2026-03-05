using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using task.Data.Entities;

namespace task.Data.Configurations;

class OfficeConfiguration : IEntityTypeConfiguration<Office>
{
    public void Configure(EntityTypeBuilder<Office> builder)
    {
        builder.ToTable("offices");

        builder.HasKey(o => o.Id);

        builder.OwnsOne(o => o.Coordinates, c =>
        {
            c.Property(p => p.Latitude).HasColumnName("latitude").IsRequired();
            c.Property(p => p.Longitude).HasColumnName("longitude").IsRequired();
        });

        builder.HasMany(o => o.Phones)
               .WithOne(p => p.Office)
               .HasForeignKey(p => p.OfficeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(o => o.Code).HasMaxLength(100);
        builder.Property(o => o.Uuid).HasMaxLength(36);
        builder.Property(o => o.CountryCode).HasMaxLength(3).IsRequired();
        builder.Property(o => o.AddressRegion).HasMaxLength(100);
        builder.Property(o => o.AddressCity).HasMaxLength(100);
        builder.Property(o => o.AddressStreet).HasMaxLength(100);
        builder.Property(o => o.AddressHouseNumber).HasMaxLength(100);
        builder.Property(o => o.WorkTime).HasMaxLength(500).IsRequired();

        builder.HasIndex(o => o.Code);
        builder.HasIndex(o => o.CityCode);
        builder.HasIndex(o => o.Uuid);
        builder.HasIndex(o => o.CountryCode);
    }
}
