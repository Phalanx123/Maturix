using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Maturix.Sample.Scenarios;

/// <summary>
/// Resolves scenarios and runs one or all of them, depending on the mode.
/// </summary>
internal sealed class ScenarioRunner
{
    private readonly IReadOnlyList<IEndpointScenario> _scenarios;
    private readonly ILogger<ScenarioRunner> _logger;

    public ScenarioRunner(IEnumerable<IEndpointScenario> scenarios, ILogger<ScenarioRunner> logger)
    {
        _scenarios = scenarios.ToList();
        _logger = logger;
    }

    public async Task RunAsync(string mode, ScenarioInput input)
    {
        if (string.Equals(mode, "all", StringComparison.OrdinalIgnoreCase))
        {
            foreach (var s in _scenarios)
            {
                _logger.LogInformation("Running scenario: {Key}", s.Key);
                await s.RunAsync(input);
                Console.WriteLine(new string('-', 60));
            }
            return;
        }

        var scenario = _scenarios.FirstOrDefault(s => string.Equals(s.Key, mode, StringComparison.OrdinalIgnoreCase));
        if (scenario is null)
        {
            Console.WriteLine($"Unknown mode '{mode}'. Available: all | {string.Join(" | ", _scenarios.Select(s => s.Key))}");
            return;
        }

        _logger.LogInformation("Running scenario: {Key}", scenario.Key);
        await scenario.RunAsync(input);
    }
}