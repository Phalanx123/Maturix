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
internal sealed class QualityReportsScenario : IEndpointScenario
{
    private readonly MaturixClient _client;
    private readonly ILogger<QualityReportsScenario> _logger;

    public QualityReportsScenario(MaturixClient client, ILogger<QualityReportsScenario> logger)
    {
        _client = client;
        _logger = logger;
    }

    public string Key => "reports";

    public async Task RunAsync(ScenarioInput input, CancellationToken ct = default)
    {
        Console.WriteLine("Fetching quality reports...");
        var result = await _client.GetQualityReportsAsync(ct).ConfigureAwait(false);

        result.Switch(
            reports =>
            {
                Console.WriteLine($"Received {reports.Count} quality reports:\n");
                foreach (var report in reports)
                {
                    Console.WriteLine($"Report {report.Id} – Production: {report.ProductionId} – Compound: {report.Compound}");
                }
            },
            error =>
            {
                _logger.LogError("Error fetching reports: {Error}", error);
                Console.WriteLine($"Error fetching reports: {error}");
            });
    }
}