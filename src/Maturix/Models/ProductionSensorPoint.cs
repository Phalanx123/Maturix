using System.Text.Json.Serialization;

namespace Maturix.Models;
/// <summary>
/// Single time-series sample from the Production Unit Dashboard response.
/// Maturix returns these within the "Sensordata" array.
/// </summary>
public class ProductionSensorPoint
{
    /// <summary>
    /// Unix timestamp (seconds) for this sample.
    /// </summary>
    [JsonPropertyName("unix")]
    public long Unix { get; set; }

    /// <summary>
    /// Temperature at this moment. Unit is °C as supplied by Maturix.
    /// </summary>
    [JsonPropertyName("temp")]
    public double Temp { get; set; }

    /// <summary>
    /// Calculated strength (may be absent/null if not available at this sample).
    /// </summary>
    [JsonPropertyName("strength")]
    public double? Strength { get; set; }

    /// <summary>
    /// Accumulated maturity for this sample (seconds).
    /// Not always present; null when not emitted by the API at this point.
    /// </summary>
    [JsonPropertyName("maturity_seconds")]
    public long? MaturitySeconds { get; set; }
}