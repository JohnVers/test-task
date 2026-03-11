using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using task.Entities;

namespace task.Data;

internal sealed class OfficeConfiguration : IEntityTypeConfiguration<Office>
{
    public void Configure(EntityTypeBuilder<Office> builder)
    {
        builder.ToTable("offices");

        builder.HasKey(x => x.Id);

        builder.HasMany(o => o.Phones)
               .WithOne(p => p.Office)
               .HasForeignKey(p => p.OfficeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(o => o.CityCode);
        builder.HasIndex(o => o.Code);

        builder.OwnsOne(o => o.Coordinates);

        builder.Property(o => o.CityCode).IsRequired();
        builder.Property(o => o.Code).HasMaxLength(25).IsRequired();
        builder.Property(o => o.Uuid).HasMaxLength(36);
        builder.Property(o => o.CountryCode).HasMaxLength(3);
        builder.Property(o => o.AddressRegion).HasMaxLength(250);
        builder.Property(o => o.AddressCity).HasMaxLength(250);
        builder.Property(o => o.AddressStreet).HasMaxLength(250);
        builder.Property(o => o.AddressHouseNumber).HasMaxLength(500);
        builder.Property(o => o.AddressApartment).HasMaxLength(250);
        builder.Property(o => o.WorkTime).HasMaxLength(250).IsRequired(false);
    }
}
