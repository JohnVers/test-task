using Microsoft.EntityFrameworkCore;
using task.Data.Configurations;
using task.Data.Entities;

namespace task.Data;

class DellinDictionaryDbContext(DbContextOptions<DellinDictionaryDbContext> options) : DbContext(options)
{
    public DbSet<Office> Offices { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new OfficeConfiguration());
        builder.ApplyConfiguration(new PhoneConfiguration());
    }
}
