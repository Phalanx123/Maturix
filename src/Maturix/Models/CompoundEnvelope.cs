using System.Text.Json.Serialization;

namespace Maturix.Models;

/// <summary>
/// Represents an envelope for Compound Quality Reports returned by the Maturix API.
/// </summary>
public class CompoundEnvelope
{
    /// <summary>
    /// Gets or sets the list of sensor production data.
    /// </summary>
    [JsonPropertyName("Compounds")]
    public List<Compound>? Compounds { get; set; }
}