using System.Reflection;
using Microsoft.EntityFrameworkCore;
using StranaKart.Assignment.Domain.Entities;

namespace StranaKart.Assignment.Infrastructure;

internal class DellinDictionaryDbContext: DbContext
{
    public required DbSet<Office> Offices { get; set; }

    public DellinDictionaryDbContext(DbContextOptions<DellinDictionaryDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}