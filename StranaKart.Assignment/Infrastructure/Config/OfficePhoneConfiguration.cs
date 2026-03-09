using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StranaKart.Assignment.Domain.Entities;

namespace StranaKart.Assignment.Infrastructure.Config;

public class OfficePhoneConfiguration: IEntityTypeConfiguration<OfficePhone>
{
    public void Configure(EntityTypeBuilder<OfficePhone> builder)
    {
        builder.ToTable("office_phones");
        
        builder.HasKey(x => new { x.OfficeId, x.PhoneNumber });
        builder.Property(x => x.PhoneNumber).IsRequired();
        builder.Property(x => x.OfficeId).IsRequired();
        builder.Property(x => x.Additional).IsRequired(false);
    }
}