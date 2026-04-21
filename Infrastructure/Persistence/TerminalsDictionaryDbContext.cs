using System.Reflection;
using Microsoft.EntityFrameworkCore;
using task.Domain.Entities;

namespace task.Infrastructure.Persistence;

public sealed class TerminalsDictionaryDbContext(DbContextOptions<TerminalsDictionaryDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<Office> Offices => Set<Office>();

    public DbSet<Phone> Phones => Set<Phone>();
}
