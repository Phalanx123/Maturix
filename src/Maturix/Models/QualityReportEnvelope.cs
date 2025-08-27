using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Maturix.Models;

/// <summary>
/// Represents an envelope for Quality Reports returned by the Maturix API.
/// </summary>
public class QualityReportEnvelope
{
    /// <summary>
    /// Gets or sets the list of sensor production data.
    /// </summary>
    [JsonPropertyName("QualityReports")]
    public List<QualityReport>? QualityReports { get; set; }
}