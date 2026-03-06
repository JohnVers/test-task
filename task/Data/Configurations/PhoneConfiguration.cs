using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using task.Entities;

namespace task.Data.Configurations;

public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
{
    public void Configure(EntityTypeBuilder<Phone> builder)
    {
        builder.ToTable("phones");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Additional)
            .HasMaxLength(200);

        builder.HasIndex(e => e.OfficeId);
    }
}
