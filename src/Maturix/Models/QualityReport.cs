using System.Text.Json.Serialization;

namespace Maturix.Models
{
    /// <summary>
    /// Represents a single quality report returned by the Maturix API.  
    /// All properties map directly to the JSON fields provided by the API.  
    /// Most fields are nullable because the API may omit values or return null.
    /// </summary>
    public class QualityReport
    {
        [JsonPropertyName("ID")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("Name")]
        public string? Name { get; set; }
        [JsonPropertyName("ProductionID")]
        public string? ProductionId { get; set; }
        [JsonPropertyName("WorkstationID")]
        public string? WorkstationId { get; set; }
        [JsonPropertyName("WorkstationName")]
        public string? WorkstationName { get; set; }
        [JsonPropertyName("ZoneID")]
        public string? ZoneId { get; set; }
        [JsonPropertyName("ZoneName")]
        public string? ZoneName { get; set; }
        [JsonPropertyName("Status")]
        public string? Status { get; set; }
        [JsonPropertyName("SensorHRID")]
        public string? SensorHRID { get; set; }
        [JsonPropertyName("SensorDataStart")]
        public long? SensorDataStart { get; set; }
        [JsonPropertyName("SensorDataStop")]
        public long? SensorDataStop { get; set; }
        [JsonPropertyName("CurrentAvgTemp")]
        public double? CurrentAvgTemp { get; set; }
        [JsonPropertyName("CurrentMaturityHour")]
        public double? CurrentMaturityHour { get; set; }
        [JsonPropertyName("TargetMaturityHour")]
        public double? TargetMaturityHour { get; set; }
        [JsonPropertyName("TargetMaturityUnix")]
        public long? TargetMaturityUnix { get; set; }
        [JsonPropertyName("Compound")]
        public string? Compound { get; set; }
    }
}