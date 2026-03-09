using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StranaKart.Assignment.Domain.Entities;

namespace StranaKart.Assignment.Infrastructure.Config;

internal class OfficeEntityConfiguration: IEntityTypeConfiguration<Office>
{
    public void Configure(EntityTypeBuilder<Office> builder)
    {
        builder.ToTable("offices");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        
        builder.OwnsOne(x => x.Coordinates, coordinates =>
        {
            coordinates.Property(x => x.Latitude).HasColumnName("latitude");
            coordinates.Property(x => x.Longitude).HasColumnName("longitude");
        });

        builder.HasMany(x => x.Phones)
            .WithOne()
            .HasForeignKey(x => x.OfficeId);
        
        builder.HasIndex(x => x.Uuid).IsUnique();
        builder.HasIndex(x => x.CityCode);
        builder.HasIndex(x => new { x.CityCode, x.Type});
        builder.HasIndex(x => x.AddressCity);
        builder.HasIndex(x => new { x.AddressCity, x.Type });
    }
}
