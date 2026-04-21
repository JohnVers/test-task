using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using task.Domain.Entities;

namespace task.Infrastructure.Persistence.Configurations;

public sealed class OfficeConfiguration : IEntityTypeConfiguration<Office>
{
    public void Configure(EntityTypeBuilder<Office> builder)
    {
        builder.ToTable("offices");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).HasMaxLength(64);
        builder.Property(x => x.Uuid).HasMaxLength(128);
        builder.Property(x => x.CountryCode).HasMaxLength(8).IsRequired();
        builder.Property(x => x.AddressRegion).HasMaxLength(256);
        builder.Property(x => x.AddressCity).HasMaxLength(256);
        builder.Property(x => x.AddressStreet).HasMaxLength(256);
        builder.Property(x => x.AddressHouseNumber).HasMaxLength(64);
        builder.Property(x => x.WorkTime).HasMaxLength(512).IsRequired();

        builder.ComplexProperty(x => x.Coordinates, complex =>
        {
            complex.Property(x => x.Latitude).HasColumnName("latitude");
            complex.Property(x => x.Longitude).HasColumnName("longitude");
        });

        builder.Property<DateTimeOffset>("ImportedAt");
        builder.Property<string>("SourceFile").HasMaxLength(256);

        builder.HasIndex(x => x.CityCode);
        builder.HasIndex(x => new { x.AddressCity, x.AddressRegion });
        builder.HasIndex(x => x.Uuid).IsUnique(false);

        builder.HasMany(x => x.Phones)
            .WithOne(x => x.Office)
            .HasForeignKey(x => x.OfficeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
