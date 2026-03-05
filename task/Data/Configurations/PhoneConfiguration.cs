using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using task.Data.Entities;

namespace task.Data.Configurations;

class PhoneConfiguration : IEntityTypeConfiguration<Phone>
{
    public void Configure(EntityTypeBuilder<Phone> builder)
    {
        builder.ToTable("phones");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PhoneNumber).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Additional).HasMaxLength(200);

        builder.HasOne(p => p.Office)
               .WithMany(o => o.Phones)
               .HasForeignKey(p => p.OfficeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.OfficeId);
        builder.HasIndex(p => p.PhoneNumber);
    }
}
