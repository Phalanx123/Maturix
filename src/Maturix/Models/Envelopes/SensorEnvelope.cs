using System.Text.Json.Serialization;

namespace Maturix.Models.Envelopes;

internal sealed class SensorsEnvelope
{
    /// <summary>
    /// Sensors returned by the Maturix API.
    /// </summary>
    [JsonPropertyName("Sensors")]
    public List<Sensor>? Sensors { get; set; }
}