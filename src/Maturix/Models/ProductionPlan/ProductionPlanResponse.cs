using System.ComponentModel;
using System.Text.Json.Serialization;
using Maturix.Converters;
using Maturix.Models.Requests;

namespace Maturix.Models.ProductionPlan;

/// <summary>
/// Production plan details as returned by the Maturix API.
/// </summary>
public class ProductionPlanResponse
{
    /// <summary>Production ID</summary>
    [JsonPropertyName("ID")]
    public string Id { get; init; } = string.Empty;

    /// <summary></summary>
    [JsonPropertyName("UnitName")]
    public string? UnitName { get; init; }

    /// <summary>
    /// Compound Name e.g. "N50 S180/5/450"
    /// Keep raw to preserve original formatting; parse downstream if needed.
    /// </summary>
    [JsonPropertyName("Compound")]
    public string? Compound { get; init; }

    /// <summary>
    /// Friendly Production Name
    /// </summary>
    [JsonPropertyName("ProductionID")]
    public string? ProductionId { get; init; }

    /// <summary>
    /// Zone ID
    /// </summary>
    [JsonPropertyName("ZoneID")]
    public string? ZoneId { get; init; }

    /// <summary>
    /// Zone Name Friendly
    /// </summary>
    [JsonPropertyName("ZoneName")]
    public string? ZoneName { get; init; }

    /// <summary>
    /// Workstation ID
    /// </summary>
    [JsonPropertyName("WorkstationID")]
    public string? WorkstationId { get; init; }

    /// <summary>
    /// Friendly Workstation Name
    /// </summary>
    [JsonPropertyName("WorkstationName")]
    public string? WorkstationName { get; init; }

    /// <summary>
    /// Production Start in Unix time
    /// </summary>
    [JsonPropertyName("ProductionStart")]
    public string? ProductionStart { get; init; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonPropertyName("Status")]
    public int Status { get; init; }

    /// <summary>
    /// Target Strength as string e.g. "45.0".
    /// </summary>
    [JsonPropertyName("Strength")]
    public string? Strength { get; init; }

    /// <summary>
    /// 1 = true, 2 = false
    /// </summary>
    [JsonPropertyName("AutoStop")]
    [JsonConverter(typeof(IntToBoolJsonConverter))]
    public bool AutoStop { get; init; }

    /// <summary>
    /// Schedule Time
    /// </summary>
    [JsonPropertyName("ScheduleTime")]
    public string? ScheduleTimeRaw { get; init; }

    /// <summary>
    /// Schedule Hour
    /// </summary>
    [JsonPropertyName("ScheduleHour")]
    public string? ScheduleHour { get; init; }

    /// <summary>
   /// Production Date in UTC as ISO 8601 string, or null if not set.
    /// </summary>
    [JsonPropertyName("ProductionDate")]
    public string? ProductionDate { get; init; }

    /// <summary>
   /// Label colour e.g. "red", "green", "blue", or null for no label.
    /// </summary>
    [JsonPropertyName("Label")]
    public ProductionPlanLabelEnum? Label { get; init; }

    /// <summary>"Notes"</summary>
    [JsonPropertyName("Notes")]
    public string? Notes { get; init; }
}