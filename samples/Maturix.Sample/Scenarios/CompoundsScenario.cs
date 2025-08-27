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
internal sealed class CompoundsScenario : IEndpointScenario
{
    private readonly MaturixClient _client;
    private readonly ILogger<CompoundsScenario> _logger;

    public CompoundsScenario(MaturixClient client, ILogger<CompoundsScenario> logger)
    {
        _client = client;
        _logger = logger;
    }

    public string Key => "compounds";

    public async Task RunAsync(ScenarioInput input, CancellationToken ct = default)
    {
        Console.WriteLine("Fetching compounds...");
        var result = await _client.GetCompoundsAsync(ct).ConfigureAwait(false);

        result.Switch(
            compounds =>
            {
                Console.WriteLine($"Received {compounds.Count} compounds:\n");
                foreach (var compound in compounds)
                {
                    Console.WriteLine($"Compound {compound.Id} – {compound.Name} - Target Strength - {compound.TargetStrength}");
                }
            },
            error =>
            {
                _logger.LogError("Error fetching compounds: {Error}", error);
                Console.WriteLine($"Error fetching compounds: {error}");
            });
    }
}