using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Domain.Repositories;
using RealEstate.Infrastructure.Configuration;
using RealEstate.Infrastructure.Data;
using RealEstate.Infrastructure.Repositories;

namespace RealEstate.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configuración de MongoDB
        var mongoSettings = configuration.GetSection(MongoDbSettings.SectionName).Get<MongoDbSettings>();

        if (mongoSettings == null)
        {
            throw new InvalidOperationException("MongoDB settings are not configured properly");
        }

        services.AddSingleton(mongoSettings);
        services.AddSingleton<IMongoDbContext, MongoDbContext>();

        // Registrar repositorios
        services.AddScoped<IPropertyRepository, PropertyRepository>();

        return services;
    }
}