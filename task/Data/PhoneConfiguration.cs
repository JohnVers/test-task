using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using task.Entities;

namespace task.Data;

internal sealed class PhoneConfiguration : IEntityTypeConfiguration<Phone>
{
    public void Configure(EntityTypeBuilder<Phone> builder)
    {
        builder.ToTable("phones");

        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.Office)
               .WithMany(o => o.Phones)
               .HasForeignKey(p => p.OfficeId);

        builder.Property(p => p.PhoneNumber).HasMaxLength(20).IsRequired();
        builder.Property(p => p.Additional).HasMaxLength(250);
    }
}
