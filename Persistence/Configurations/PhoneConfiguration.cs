using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
{
    public void Configure(EntityTypeBuilder<Phone> builder)
    {
        builder.ToTable("phone");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.Property(p => p.OfficeId).HasColumnName("office_id").IsRequired();
        builder.Property(p => p.PhoneNumber).HasColumnName("phone_number").IsRequired();
        builder.Property(p => p.Additional).HasColumnName("additional").IsRequired(false);
    }
}