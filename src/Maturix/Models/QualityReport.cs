using System.Text.Json.Serialization;

namespace Maturix.Models;

/// <summary>
/// Represents a single quality report returned from the Maturix API.
/// This model maps directly to the JSON fields in the API response.
/// </summary>
public class QualityReport
{
    /// <summary>
    /// Unique identifier of the quality report (stringified numeric ID).
    /// </summary>
    [JsonPropertyName("ID")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Human-readable name or label for the report (may be empty or null).
    /// </summary>
    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    /// <summary>
    /// Identifier of the related production batch/job.
    /// </summary>
    [JsonPropertyName("ProductionID")]
    public string? ProductionId { get; set; }

    /// <summary>
    /// Unique identifier of the workstation where data was recorded.
    /// </summary>
    [JsonPropertyName("WorkstationID")]
    public string? WorkstationId { get; set; }

    /// <summary>
    /// Friendly display name of the workstation.
    /// </summary>
    [JsonPropertyName("WorkstationName")]
    public string? WorkstationName { get; set; }

    /// <summary>
    /// Identifier of the zone (location grouping within the plant).
    /// </summary>
    [JsonPropertyName("ZoneID")]
    public string? ZoneId { get; set; }

    /// <summary>
    /// Friendly display name of the zone.
    /// </summary>
    [JsonPropertyName("ZoneName")]
    public string? ZoneName { get; set; }

    /// <summary>
    /// Status code of the report (API returns a string code, e.g. "2").
    /// </summary>
    [JsonPropertyName("Status")]
    public string? Status { get; set; }

    /// <summary>
    /// Human-readable ID of the sensor device linked to this report.
    /// </summary>
    [JsonPropertyName("SensorHRID")]
    public string? SensorHRID { get; set; }

    /// <summary>
    /// Unix timestamp when sensor data collection started.
    /// </summary>
    [JsonPropertyName("SensorDataStart")]
    public long? SensorDataStart { get; set; }

    /// <summary>
    /// Unix timestamp when sensor data collection stopped (null if ongoing).
    /// </summary>
    [JsonPropertyName("SensorDataStop")]
    public long? SensorDataStop { get; set; }

    /// <summary>
    /// Current average temperature recorded across the sensor’s readings (°C).
    /// </summary>
    [JsonPropertyName("CurrentAvgTemp")]
    public double? CurrentAvgTemp { get; set; }

    /// <summary>
    /// Current calculated maturity hours (degree-hours).
    /// </summary>
    [JsonPropertyName("CurrentMaturityHour")]
    public double? CurrentMaturityHour { get; set; }

    /// <summary>
    /// Target maturity hours required to reach the desired strength.
    /// </summary>
    [JsonPropertyName("TargetMaturityHour")]
    public double? TargetMaturityHour { get; set; }

    /// <summary>
    /// Target maturity point expressed as a Unix timestamp.
    /// </summary>
    [JsonPropertyName("TargetMaturityUnix")]
    public long? TargetMaturityUnix { get; set; }

    /// <summary>
    /// Compound mix ratio or designation (e.g. "60/20").
    /// </summary>
    [JsonPropertyName("Compound")]
    public string? Compound { get; set; }
}