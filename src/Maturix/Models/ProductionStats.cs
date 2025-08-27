using System.Text.Json.Serialization;

namespace Maturix.Models;

/// <summary>
/// Detailed production/unit statistics as returned by the dashboard endpoint.
/// Types are chosen to reflect the wire format shown by the API:
/// - Some numerics are sent as strings (e.g., "TargetMaturityHour", "TargetStrength", "Status", "AutoStop").
/// - Epoch values are represented as Unix timestamps (seconds).
/// </summary>
public sealed class ProductionStats
{
    /// <summary>Which maturity function is used (e.g., Nurse-Saul vs Arrhenius), numeric flag.</summary>
    [JsonPropertyName("MaturityFunction")]
    public int MaturityFunction { get; set; }

    /// <summary>Start of sensor data (Unix seconds).</summary>
    [JsonPropertyName("SensorDataStart")]
    public long SensorDataStart { get; set; }

    /// <summary>End of sensor data (Unix seconds).</summary>
    [JsonPropertyName("SensorDataStop")]
    public long? SensorDataStop { get; set; }

    /// <summary>Minimum temperature across the series.</summary>
    [JsonPropertyName("TempMin")]
    public double TempMin { get; set; }

    /// <summary>Maximum temperature across the series.</summary>
    [JsonPropertyName("TempMax")]
    public double TempMax { get; set; }

    /// <summary>Average temperature across the series.</summary>
    [JsonPropertyName("TempAvg")]
    public double TempAvg { get; set; }

    /// <summary>Accumulated maturity, in hours.</summary>
    [JsonPropertyName("MaturityHour")]
    public double MaturityHour { get; set; }

    /// <summary>Estimated time of target maturity (Unix seconds).</summary>
    [JsonPropertyName("ETA")]
    public long Eta { get; set; }

    /// <summary>Overall equivalent age maturity flag/value (API returns an int).</summary>
    [JsonPropertyName("EquivilantAgeMaturityOverall")]
    public double EquivilantAgeMaturityOverall { get; set; }

    /// <summary>Current equivalent age maturity flag/value (API returns an int).</summary>
    [JsonPropertyName("EquivilantAgeMaturityNow")]
    public double EquivilantAgeMaturityNow { get; set; }

    /// <summary>Equivalent age maturity seconds (accumulated). Use long for headroom.</summary>
    [JsonPropertyName("EquivilantAgeMaturitySeconds")]
    public long EquivilantAgeMaturitySeconds { get; set; }

    /// <summary>Nurse–Saul maturity (hours).</summary>
    [JsonPropertyName("NurseSauMaturityHour")]
    public double NurseSauMaturityHour { get; set; }

    /// <summary>Target maturity (hours) – sent as a string (e.g., "19.5").</summary>
    [JsonPropertyName("TargetMaturityHour")]
    public string? TargetMaturityHour { get; set; }

    /// <summary>Unix time when target maturity is expected.</summary>
    [JsonPropertyName("TargetMaturityUnix")]
    public long? TargetMaturityUnix { get; set; }

    /// <summary>External ID of the production/unit.</summary>
    [JsonPropertyName("ProductionID")]
    public string? ProductionId { get; set; }

    /// <summary>Optional user-facing unit name/label.</summary>
    [JsonPropertyName("UnitName")]
    public string? UnitName { get; set; }

    /// <summary>Production date/time (Unix seconds).</summary>
    [JsonPropertyName("ProductionDate")]
    public long ProductionDate { get; set; }

    /// <summary>Optional label for the unit or pour.</summary>
    [JsonPropertyName("Label")]
    public string? Label { get; set; }

    /// <summary>Free-form notes.</summary>
    [JsonPropertyName("Notes")]
    public string? Notes { get; set; }

    /// <summary>Status code returned as a string (e.g., "3").</summary>
    [JsonPropertyName("Status")]
    public string? Status { get; set; }

    /// <summary>Latest sample timestamp (Unix seconds).</summary>
    [JsonPropertyName("CurrentUnix")]
    public long CurrentUnix { get; set; }

    /// <summary>Current calculated strength at <see cref="CurrentUnix"/>.</summary>
    [JsonPropertyName("CurrentStrength")]
    public double CurrentStrength { get; set; }

    /// <summary>Current measured temperature at <see cref="CurrentUnix"/>.</summary>
    [JsonPropertyName("CurrentTemp")]
    public double CurrentTemp { get; set; }

    /// <summary>Unix time of the last measurement received.</summary>
    [JsonPropertyName("LastMeasurementUnix")]
    public long LastMeasurementUnix { get; set; }

    /// <summary>Target strength value as a string (API sends quoted numerics).</summary>
    [JsonPropertyName("TargetStrength")]
    public string? TargetStrength { get; set; }

    /// <summary>Sensor identifier (e.g., device code).</summary>
    [JsonPropertyName("Sensor")]
    public string? Sensor { get; set; }

    /// <summary>Human-friendly sensor name if provided; can be null.</summary>
    [JsonPropertyName("SensorName")]
    public string? SensorName { get; set; }

    /// <summary>Autostop flag as a string (e.g., "1").</summary>
    [JsonPropertyName("AutoStop")]
    public string? AutoStop { get; set; }

    /// <summary>Locked flag, often null; preserve as string? to match payload.</summary>
    [JsonPropertyName("Locked")]
    public string? Locked { get; set; }

    /// <summary>Workstation identifier (string in payload).</summary>
    [JsonPropertyName("WorkstationID")]
    public string? WorkstationId { get; set; }

    /// <summary>Display name of the workstation.</summary>
    [JsonPropertyName("WorkstationName")]
    public string? WorkstationName { get; set; }

    /// <summary>Zone identifier (string in payload).</summary>
    [JsonPropertyName("ZoneID")]
    public string? ZoneId { get; set; }

    /// <summary>Display name of the zone.</summary>
    [JsonPropertyName("ZoneName")]
    public string? ZoneName { get; set; }

    /// <summary>Dutch-weighted maturity seconds; null when not calculated.</summary>
    [JsonPropertyName("DutchWeightedMaturitySeconds")]
    public long? DutchWeightedMaturitySeconds { get; set; }
}
