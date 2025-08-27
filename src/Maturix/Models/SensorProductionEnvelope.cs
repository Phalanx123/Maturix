using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Maturix.Models;

/// <summary>
/// Represents an envelope for sensor production data returned by the Maturix API.
/// </summary>
public abstract class SensorProductionEnvelope
{
    /// <summary>
    /// Gets or sets the list of sensor production data.
    /// </summary>
    [JsonPropertyName("CurrentProductions")]
    public List<SensorProductionData>? ProductionData { get; set; }
}