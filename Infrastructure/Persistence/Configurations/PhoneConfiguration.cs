using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using task.Domain.Entities;

namespace task.Infrastructure.Persistence.Configurations;

public sealed class PhoneConfiguration : IEntityTypeConfiguration<Phone>
{
    public void Configure(EntityTypeBuilder<Phone> builder)
    {
        builder.ToTable("phones");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PhoneNumber).HasMaxLength(64).IsRequired();
        builder.Property(x => x.Additional).HasMaxLength(256);

        builder.HasIndex(x => x.OfficeId);
    }
}
