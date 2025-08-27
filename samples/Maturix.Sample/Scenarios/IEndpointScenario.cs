using System.Threading;
using System.Threading.Tasks;

namespace Maturix.Sample.Scenarios;

/// <summary>
/// Contract for a runnable sample scenario (one API endpoint or a small group).
/// </summary>
internal interface IEndpointScenario
{
    /// <summary>Unique key used to select this scenario from args (e.g., "reports").</summary>
    string Key { get; }

    /// <summary>Execute the scenario with provided input.</summary>
    Task RunAsync(ScenarioInput input, CancellationToken ct = default);
}