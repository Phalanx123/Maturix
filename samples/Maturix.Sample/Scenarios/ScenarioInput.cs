namespace Maturix.Sample.Scenarios;

/// <summary>
/// Simple input bag so scenarios can share common identifiers without tight coupling.
/// Add more fields as your endpoints require them.
/// </summary>
internal sealed class ScenarioInput
{
    public ScenarioInput(string locationId, string productionId)
    {
        LocationId = locationId;
        ProductionId = productionId;
    }

    public string LocationId { get; }
    public string ProductionId { get; }
}