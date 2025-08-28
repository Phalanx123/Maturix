using System;
using System.Threading;
using System.Threading.Tasks;
using Maturix;
using Maturix.Clients;
using Maturix.Models.Requests;
using Microsoft.Extensions.Logging;

namespace Maturix.Sample.Scenarios;

/// <summary>
/// Demonstrates calling GetQualityReportsAsync.
/// </summary>
internal sealed class NewProductionPlanScenario : IEndpointScenario
{
    private readonly MaturixClient _client;
    private readonly ILogger<CompoundsScenario> _logger;

    public NewProductionPlanScenario(MaturixClient client, ILogger<CompoundsScenario> logger)
    {
        _client = client;
        _logger = logger;
    }

    public string Key => "newProductionPlan";

    public async Task RunAsync(ScenarioInput input, CancellationToken ct = default)
    {
        var p = new NewProductionPlanEntryRequest
        {
            AutoStop = true,
            CompoundId = 3105,
            Label = ProductionPlanLabelEnum.Blue,
            ProductionId = "ScenarioTest",
            ProductionUnix = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds(),
            WorkstationId = 2468,
            Strength = 15
        };


        Console.WriteLine("Creating new production plan...");
        var result = await _client.NewProductionPlan(p, ct).ConfigureAwait(false);

        result.Switch(
            success =>
            {
                if (success)
                {
                    Console.WriteLine($"Added New Production Plan {success}");
                    return;
                }

                Console.WriteLine("Failed to add new Production Plan");
            },
            error =>
            {
                _logger.LogError("Error adding new Production Plan {Error}", error);
                Console.WriteLine($"Error adding new Production Plan {error}");
            });
    }
}