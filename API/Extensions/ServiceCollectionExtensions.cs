using Domain.Configurations;

namespace Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration.GetSection(nameof(JwtAuthorizationConfiguration)).Get<JwtAuthorizationConfiguration>()!);

        return services;
    }
}