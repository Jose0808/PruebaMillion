using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Application.Services;
using System.Reflection;

namespace RealEstate.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registrar servicios de aplicación
        services.AddScoped<IPropertyService, PropertyService>();

        // Registrar validadores
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}