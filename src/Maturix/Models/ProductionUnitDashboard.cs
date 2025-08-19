using System.Text.Json.Serialization;

namespace Maturix.Models
{
    /// <summary>
    /// Represents the dashboard data returned by the ProductionUnitDashboard
    /// endpoint.  
    /// The <see cref="Stats"/> property contains aggregated statistics for the
    /// production unit, while <see cref="SensorData"/> contains a series of
    /// data points captured over time.
    /// </summary>
    public class ProductionUnitDashboard
    {
        [JsonPropertyName("Stats")]
        public ProductionUnitStats? Stats { get; set; }

        [JsonPropertyName("Sensordata")]
        public List<SensorDataEntry>? SensorData { get; set; }
    }

    /// <summary>
    /// Contains statistical information about a production unit.  
    /// All properties map directly to the JSON fields provided by the API.
    /// </summary>
    public class ProductionUnitStats
    {
        [JsonPropertyName("MaturityFunction")]
        public int MaturityFunction { get; set; }

        [JsonPropertyName("SensorDataStart")]
        public long? SensorDataStart { get; set; }

        [JsonPropertyName("SensorDataStop")]
        public long? SensorDataStop { get; set; }

        [JsonPropertyName("TempMin")]
        public double? TempMin { get; set; }

        [JsonPropertyName("TempMax")]
        public double? TempMax { get; set; }

        [JsonPropertyName("TempAvg")]
        public double? TempAvg { get; set; }

        [JsonPropertyName("MaturityHour")]
        public double? MaturityHour { get; set; }

        [JsonPropertyName("ETA")]
        public long? Eta { get; set; }

        [JsonPropertyName("EquivilantAgeMaturityOverall")]
        public double? EquivalentAgeMaturityOverall { get; set; }

        [JsonPropertyName("EquivilantAgeMaturityNow")]
        public double? EquivalentAgeMaturityNow { get; set; }

        [JsonPropertyName("EquivilantAgeMaturitySeconds")]
        public long? EquivalentAgeMaturitySeconds { get; set; }

        [JsonPropertyName("NurseSauMaturityHour")]
        public double? NurseSauMaturityHour { get; set; }

        [JsonPropertyName("TargetMaturityHour")]
        public double? TargetMaturityHour { get; set; }

        [JsonPropertyName("TargetMaturityUnix")]
        public long? TargetMaturityUnix { get; set; }

        [JsonPropertyName("DutchWeightedMaturitySeconds")]
        public long? DutchWeightedMaturitySeconds { get; set; }

        [JsonPropertyName("ProductionID")]
        public string? ProductionId { get; set; }

        [JsonPropertyName("ProductionDate")]
        public long? ProductionDate { get; set; }

        [JsonPropertyName("UnitName")]
        public string? UnitName { get; set; }

        [JsonPropertyName("Label")]
        public string? Label { get; set; }

        [JsonPropertyName("Notes")]
        public string? Notes { get; set; }

        [JsonPropertyName("Status")]
        public string? Status { get; set; }

        [JsonPropertyName("CurrentUnix")]
        public long? CurrentUnix { get; set; }

        [JsonPropertyName("CurrentStrength")]
        public double? CurrentStrength { get; set; }

        [JsonPropertyName("CurrentTemp")]
        public double? CurrentTemp { get; set; }

        [JsonPropertyName("LastMeasurementUnix")]
        public long? LastMeasurementUnix { get; set; }

        [JsonPropertyName("TargetStrength")]
        public double? TargetStrength { get; set; }

        [JsonPropertyName("Sensor")]
        public string? Sensor { get; set; }

        [JsonPropertyName("SensorName")]
        public string? SensorName { get; set; }

        [JsonPropertyName("AutoStop")]
        public string? AutoStop { get; set; }

        [JsonPropertyName("Locked")]
        public string? Locked { get; set; }

        [JsonPropertyName("WorkstationID")]
        public string? WorkstationId { get; set; }

        [JsonPropertyName("WorkstationName")]
        public string? WorkstationName { get; set; }

        [JsonPropertyName("ZoneID")]
        public string? ZoneId { get; set; }

        [JsonPropertyName("ZoneName")]
        public string? ZoneName { get; set; }
    }

    /// <summary>
    /// Represents a single sensor data entry.  
    /// Each entry contains a timestamp along with optional temperature,
    /// strength and maturity values. Null values indicate that the field was
    /// not provided by the API.
    /// </summary>
    public class SensorDataEntry
    {
        [JsonPropertyName("unix")]
        public long Unix { get; set; }

        [JsonPropertyName("temp")]
        public double? Temp { get; set; }

        [JsonPropertyName("strength")]
        public double? Strength { get; set; }

        [JsonPropertyName("maturity_seconds")]
        public long? MaturitySeconds { get; set; }
    }
}