using System;
using System.Threading;
using System.Threading.Tasks;
using Maturix;
using Maturix.Clients;
using Microsoft.Extensions.Logging;

namespace Maturix.Sample.Scenarios;

/// <summary>
/// Demonstrates calling GetQualityReportsAsync.
/// </summary>
internal sealed class ProductionPlanScenario : IEndpointScenario
{
    private readonly MaturixClient _client;
    private readonly ILogger<ProductionPlanScenario> _logger;

    public ProductionPlanScenario(MaturixClient client, ILogger<ProductionPlanScenario> logger)
    {
        _client = client;
        _logger = logger;
    }

    public string Key => "Plan Productions";

    public async Task RunAsync(ScenarioInput input, CancellationToken ct = default)
    {
        Console.WriteLine($"Fetching {Key}...");
        var result = await _client.GetProductionPlans(ct).ConfigureAwait(false);

        result.Switch(
            planProduction =>
            {
                Console.WriteLine($"Received {planProduction.Count} {Key}:\n");
                foreach (var plan in planProduction)
                {
                    Console.WriteLine($"{Key} {plan.Id} – {plan.ProductionId}");
                }
            },
            error =>
            {
                _logger.LogError("Error fetching {S}: {Error}", Key, error);
                Console.WriteLine($"Error fetching {Key}: {error}");
            });
    }
}