using System;
using System.Threading;
using System.Threading.Tasks;
using Maturix.Clients;
using Microsoft.Extensions.Logging;

namespace Maturix.Sample.Scenarios;

/// <summary>
/// Demonstrates calling GetProductionUnitDashboardAsync.
/// </summary>
internal sealed class ProductionUnitScenario : IEndpointScenario
{
    private readonly MaturixClient _client;
    private readonly ILogger<ProductionUnitScenario> _logger;

    public ProductionUnitScenario(MaturixClient client, ILogger<ProductionUnitScenario> logger)
    {
        _client = client;
        _logger = logger;
    }

    public string Key => "dashboard";

    public async Task RunAsync(ScenarioInput input, CancellationToken ct = default)
    {
        Console.WriteLine("Fetching production dashboard...");
        var result = await _client.GetProductionUnitAsync(input.ProductionId, ct).ConfigureAwait(false);

        result.Switch(
            dashboard =>
            {
                var stats = dashboard.Stats;
                Console.WriteLine(stats != null
                    ? $"Production {stats.ProductionId}: Current strength {stats.CurrentStrength} at temperature {stats.CurrentTemp}°C"
                    : "No stats available.");

                if (dashboard.SensorData != null)
                {
                    Console.WriteLine($"Received {dashboard.SensorData.Count} sensor data points.");
                }
            },
            error =>
            {
                _logger.LogError("Error fetching dashboard: {Error}", error);
                Console.WriteLine($"Error fetching dashboard: {error}");
            });
    }
}