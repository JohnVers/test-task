using System.Reflection;
using Microsoft.EntityFrameworkCore;
using task.Models;

namespace task.Data;

public sealed class DellinDictionaryDbContext(DbContextOptions<DellinDictionaryDbContext> options) 
    : DbContext(options)
{
    public DbSet<Office> Offices => Set<Office>();
    public DbSet<Phone> Phones => Set<Phone>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
