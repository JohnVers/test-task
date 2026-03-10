// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using test.App;
using test.Core;


namespace TestApp
{
    public static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void Main()
        {
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            Console.WriteLine("Hello, World!");

            var dbContext = ServiceProvider.GetService<DellinDictionaryDbContext>();

            int i = 1;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");

#if DEBUG
            configurationBuilder.AddJsonFile("appsettings.Development.json");
#endif

            var configuration = configurationBuilder.Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<DellinDictionaryDbContext>(options => options.UseNpgsql(connectionString));
        }
    }

}




