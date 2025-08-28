using System.Globalization;
using System.Text.Json.Serialization;
using Maturix.Abstractions;

namespace Maturix.Models.Requests;

/// <summary>
/// Represents a production plan to be sent to the Maturix API.
/// </summary>
public sealed class NewProductionPlanEntryRequest : IQueryParamSerializable
{
    /// <summary>
    /// Production ID as a string - Friendly name
    /// </summary>
    [JsonPropertyName("ProductionID")]
    public required string ProductionId { get; init; }

    /// <summary>
    /// Workstation ID - Numeric ID of the workstation where production will occur
    /// </summary>
    [JsonPropertyName("WorkstationID")]
    public required int WorkstationId { get; init; }

    /// <summary>
    /// Compound ID
    /// </summary>

    [JsonPropertyName("CompoundID")]
    public required int CompoundId { get; init; }

    /// <summary>
    /// Target Strength
    /// </summary>
    [JsonPropertyName("Strength")]
    public required int Strength { get; init; }

    /// <summary>
    /// Auto Stop at strength
    /// </summary>
    [JsonPropertyName("AutoStop")]
    public required bool AutoStop { get; init; }

    /// <summary>
    /// Notes - Optional notes about the production plan
    /// </summary>
    [JsonPropertyName("Notes")]
    public string? Notes { get; init; }

    /// <summary>
    /// Label colour i.e. Red, Green, Blue
    /// </summary>
    [JsonPropertyName("Label")]
    public ProductionPlanLabelEnum Label { get; init; } = ProductionPlanLabelEnum.NoLabel;

    /// <summary>
    /// Timestamp of production start in Unix time (seconds since epoch)
    /// </summary>
    [JsonPropertyName("ProductionUnix")]
    public long ProductionUnix { get; set; }

    /// <summary>
    /// Explicit mapping to Maturix query names.
    /// </summary>
    public IEnumerable<KeyValuePair<string, string>> ToQueryParams()
    {
        yield return new KeyValuePair<string, string>("ProductionID", ProductionId);
        yield return new KeyValuePair<string, string>("WorkstationID",
            WorkstationId.ToString(CultureInfo.InvariantCulture));
        yield return new KeyValuePair<string, string>("CompoundID", CompoundId.ToString(CultureInfo.InvariantCulture));
        yield return new KeyValuePair<string, string>("Strength", Strength.ToString(CultureInfo.InvariantCulture));
        yield return new KeyValuePair<string, string>("AutoStop", AutoStop ? "1" : "0");
        yield return new KeyValuePair<string, string>("Notes", Notes ?? string.Empty);
        yield return new KeyValuePair<string, string>("Label", Label.ToString());
        yield return new KeyValuePair<string, string>("ProductionUnix",
            ProductionUnix.ToString(CultureInfo.InvariantCulture));
    }
}