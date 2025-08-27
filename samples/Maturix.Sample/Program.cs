using Maturix.Clients;
using Maturix.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Maturix.Sample.Configuration;
using Maturix.Sample.Scenarios;

namespace Maturix.Sample;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // Add appsettings.json (optional) and user secrets
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: true)
            .AddUserSecrets<UserSecretsMarker>()  
            .AddEnvironmentVariables();

        builder.Services
            .AddOptions<MaturixClientOptions>()
            .Bind(builder.Configuration.GetSection("Maturix"));
        builder.Services.AddMaturix(opts =>
            builder.Configuration.GetSection("Maturix").Bind(opts));
        builder.Services.AddHttpClient<MaturixClient>((sp, http) =>
        {
            var opts = sp.GetRequiredService<IOptions<MaturixClientOptions>>().Value;
            http.BaseAddress = new Uri(opts.BaseUrl);
        });
        
        builder.Services.AddTransient<IEndpointScenario, QualityReportsScenario>();
        builder.Services.AddTransient<IEndpointScenario, ProductionUnitScenario>();
        builder.Services.AddTransient<ScenarioRunner>();

        using var host = builder.Build();

        var runner = host.Services.GetRequiredService<ScenarioRunner>();
        var mode = args.Length > 0 ? args[0].Trim().ToLowerInvariant() : "all";
        var locationId = GetArgValue(args, "--locationId") ?? "LocationID";
        var productionId = GetArgValue(args, "--productionId") ?? "86820";

        await runner.RunAsync(mode, new ScenarioInput(locationId, productionId));
    }

    private static string? GetArgValue(string[] args, string key)
    {
        foreach (var a in args)
        {
            if (a.StartsWith(key + "=", StringComparison.OrdinalIgnoreCase))
                return a[(key.Length + 1)..];
        }
        return null;
    }
}
