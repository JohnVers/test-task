using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class OfficeConfiguration : IEntityTypeConfiguration<Office>
{
    public void Configure(EntityTypeBuilder<Office> builder)
    {
        builder.ToTable("office");

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedOnAdd();
        
        builder.Property(o => o.Code).HasColumnName("code").IsRequired(false);
        builder.Property(o => o.CityCode).HasColumnName("city_code").IsRequired();
        builder.Property(o => o.Uuid).HasColumnName("uuid").IsRequired(false);
        builder.Property(o => o.Type).HasColumnName("type").IsRequired(false);
        builder.Property(o => o.CountryCode).HasColumnName("country_code").IsRequired();
        builder.Property(o => o.AddressRegion).HasColumnName("address_region").IsRequired(false);
        builder.Property(o => o.AddressCity).HasColumnName("address_city").IsRequired(false);
        builder.Property(o => o.AddressStreet).HasColumnName("address_street").IsRequired(false);
        builder.Property(o => o.AddressHouseNumber).HasColumnName("address_house_number").IsRequired(false);
        builder.Property(o => o.AddressApartment).HasColumnName("address_apartment").IsRequired(false);
        builder.Property(o => o.WorkTime).HasColumnName("work_time").IsRequired();
        
        builder.OwnsOne(o => o.Coordinates, coordinates =>
        {
            coordinates.Property(c => c.Latitude).HasColumnName("latitude");
            coordinates.Property(c => c.Longitude).HasColumnName("longitude");
        });

        builder.HasOne(o => o.Phone)
            .WithOne(p => p.Office)
            .OnDelete(DeleteBehavior.Cascade);

        // Индексы
        builder.HasIndex(o => o.AddressCity);
        builder.HasIndex(o => o.AddressStreet);
    }
}