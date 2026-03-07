using Application.Services.ImportTerminalsService;
using Application.Services.ImportTerminalsService.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IImportTerminalsService, ImportTerminalsService>();
        
        return serviceCollection;
    }
}