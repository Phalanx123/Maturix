using System.Text.Json.Serialization;
using Maturix.Models.ProductionPlan;

namespace Maturix.Models.Envelopes;

/// <summary>
/// Represents an envelope for Compound Quality Reports returned by the Maturix API.
/// </summary>
public class ProductionPlanEnvelope
{
    /// <summary>
    /// Gets or sets the list of sensor production data.
    /// </summary>
    [JsonPropertyName("ProductionPlan")]
    public List<ProductionPlanResponse>? ProductionPlans { get; set; }
}