using System.Text.Json.Serialization;

namespace Maturix.Models;

/// <summary>
/// Represents sensor production data including temperatures, maturity, and status.
/// </summary>
public class SensorProductionData
{
    /// <summary>
    /// Unique identifier of the record.
    /// </summary>
    [JsonPropertyName("ID")]
    public required string Id { get; set; }

    /// <summary>
    /// Display name
    /// </summary>
    [JsonPropertyName("Name")]
    public required string Name { get; set; }

    /// <summary>
    /// Identifier of the production batch or unit.
    /// </summary>
    [JsonPropertyName("ProductionID")]
    public required string ProductionId { get; set; }

    /// <summary>
    /// Identifier of the workstation.
    /// </summary>
    [JsonPropertyName("WorkstationID")]
    public required string WorkstationId { get; set; }

    /// <summary>
    /// Name of the workstation.
    /// </summary>
    [JsonPropertyName("WorkstationName")]
    public required string WorkstationName { get; set; }

    /// <summary>
    /// Identifier of the zone.
    /// </summary>
    [JsonPropertyName("ZoneID")]
    public required string ZoneId { get; set; }

    /// <summary>
    /// Name of the zone.
    /// </summary>
    [JsonPropertyName("ZoneName")]
    public required string ZoneName { get; set; }

    /// <summary>
    /// Compound description (e.g., concrete mix).
    /// </summary>
    [JsonPropertyName("Compound")]
    public required string Compound { get; set; }

    /// <summary>
    /// Current status code.
    /// 1. Not sure
    /// 2. Active
    /// 3. Finished
    /// </summary>
    [JsonPropertyName("Status")]
    public required string Status { get; set; }

    /// <summary>
    /// Human-readable identifier of the sensor.
    /// </summary>
    [JsonPropertyName("SensorHRID")]
    public required string SensorHumanReadableId { get; set; }

    /// <summary>
    /// Unix timestamp when sensor data collection started.
    /// </summary>
    [JsonPropertyName("SensorDataStart")]
    public required string SensorDataStart { get; set; }

    /// <summary>
    /// Unix timestamp when sensor data collection stopped (nullable).
    /// </summary>
    [JsonPropertyName("SensorDataStop")]
    public string? SensorDataStop { get; set; }

    /// <summary>
    /// Current average temperature in degrees Celsius.
    /// </summary>
    [JsonPropertyName("CurrentAvgTemp")]
    public required string CurrentAvgTemp { get; set; }

    /// <summary>
    /// Current maturity value in hours.
    /// </summary>
    [JsonPropertyName("CurrentMaturityHour")]
    public required string CurrentMaturityHour { get; set; }

    /// <summary>
    /// Target strength (e.g., in MPa).
    /// </summary>
    [JsonPropertyName("TargetStrength")]
    public required string TargetStrength { get; set; }

    /// <summary>
    /// Target maturity in hours.
    /// </summary>
    [JsonPropertyName("TargetMaturityHour")]
    public required string TargetMaturityHour { get; set; }

    /// <summary>
    /// Unix timestamp of the target maturity (nullable).
    /// </summary>
    [JsonPropertyName("TargetMaturityUnix")]
    public string? TargetMaturityUnix { get; set; }

    /// <summary>
    /// Date/time when approved at dashboard (nullable).
    /// </summary>
    [JsonPropertyName("ApprovedAtDashboard")]
    public string? ApprovedAtDashboard { get; set; }

    /// <summary>
    /// Estimated time of arrival (Unix timestamp).
    /// </summary>
    [JsonPropertyName("ETA")]
    public required string Eta { get; set; }

    /// <summary>
    /// Minimum recorded temperature in degrees Celsius.
    /// </summary>
    [JsonPropertyName("MinTemp")]
    public string? MinTemp { get; set; }

    /// <summary>
    /// Maximum recorded temperature in degrees Celsius.
    /// </summary>
    [JsonPropertyName("MaxTemp")]
    public string? MaxTemp { get; set; }

    /// <summary>
    /// Current recorded temperature in degrees Celsius.
    /// </summary>
    [JsonPropertyName("CurrentTemp")]
    public string? CurrentTemp { get; set; }

    /// <summary>
    /// Current calculated strength (nullable).
    /// </summary>
    [JsonPropertyName("CurrentStrength")]
    public string? CurrentStrength { get; set; }

    /// <summary>
    /// Process identifier (e.g., curing step).
    /// </summary>
    [JsonPropertyName("Process")]
    public required string Process { get; set; }
}