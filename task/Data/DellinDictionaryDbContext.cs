using System.Reflection;
using Microsoft.EntityFrameworkCore;
using task.Entities;

namespace task.Data;

public class DellinDictionaryDbContext : DbContext
{
    public DellinDictionaryDbContext(DbContextOptions<DellinDictionaryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Office> Offices { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
