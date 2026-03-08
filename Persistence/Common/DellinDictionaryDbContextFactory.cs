using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence.Common;

public class DellinDictionaryDbContextFactory : IDesignTimeDbContextFactory<DellinDictionaryDbContext>
{
    public DellinDictionaryDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DellinDictionaryDbContext>();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"];

        optionsBuilder.UseNpgsql(connectionString);

        return new DellinDictionaryDbContext(optionsBuilder.Options);
    }
}