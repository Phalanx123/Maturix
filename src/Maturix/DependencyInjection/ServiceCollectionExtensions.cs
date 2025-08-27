using Maturix.Abstractions.Clients;
using Maturix.Abstractions.Location;
using Maturix.Clients;
using Maturix.Location;
using Microsoft.Extensions.DependencyInjection;

namespace Maturix.DependencyInjection;

/// <summary>
/// Extension methods for registering Maturix services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the Maturix client, a default in-memory Location provider, and an HttpClient named "Maturix".
    /// Host apps can replace IDefaultMaturixLocationProvider with their own implementation.
    /// </summary>
    public static IServiceCollection AddMaturix(
        this IServiceCollection services,
        Action<MaturixClientOptions> configure)
    {
        services.Configure(configure);
        services.AddHttpClient("Maturix"); // BaseAddress is set in the client ctor from options

        services.AddScoped<IDefaultMaturixLocationProvider, InMemoryDefaultMaturixLocationProvider>();
        services.AddScoped<IMaturixClientFactory, MaturixClientFactory>();
        services.AddScoped<IMaturixClient>(sp => sp.GetRequiredService<IMaturixClientFactory>().Create());

        return services;
    }
}