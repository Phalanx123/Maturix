using System.Text.Json.Serialization;
namespace Maturix.Models;



    /// <summary>
    /// Represents a maturity dataset that defines how strength develops with maturity hours.
    /// </summary>
    public class Compound
    {
        /// <summary>
        /// Unique identifier of the dataset.
        /// </summary>
        [JsonPropertyName("ID")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Name of the dataset (e.g., "180/10", "60/20").
        /// Typically describes mix ratio or testing condition.
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Defines the type of maturity function used (e.g., Nurse-Saul = 1).
        /// </summary>
        [JsonPropertyName("MaturityFunction")]
        public int MaturityFunction { get; set; }

        /// <summary>
        /// The target maturity (in hours) required to reach target strength.
        /// </summary>
        [JsonPropertyName("TargetMaturityHour")]
        public string? TargetMaturityHour { get; set; }

        /// <summary>
        /// The target strength (in MPa) at the target maturity hour.
        /// </summary>
        [JsonPropertyName("TargetStrength")]
        public string? TargetStrength { get; set; }

        /// <summary>
        /// The maximum compressive strength (in MPa) expected from this dataset.
        /// </summary>
        [JsonPropertyName("MaxStrength")]
        public string? MaxStrength { get; set; }

        /// <summary>
        /// Strength development dataset represented as (MaturityHour, Strength) pairs.
        /// Example: [ [10,2], [12,3], [15,20] ]
        /// </summary>
        [JsonPropertyName("Dataset")]
        public List<List<double>> Dataset { get; set; } = new();

        /// <summary>
        /// Production classes associated with this dataset (if any).
        /// </summary>
        [JsonPropertyName("ProductionClasses")]
        public object? ProductionClasses { get; set; }

        /// <summary>
        /// Parameters associated with this dataset (empty array if none).
        /// </summary>
        [JsonPropertyName("Parameters")]
        public List<object> Parameters { get; set; } = new();

        /// <summary>
        /// Unix timestamp representing when this dataset was created.
        /// May be null if not provided.
        /// </summary>
        [JsonPropertyName("CreatedAt")]
        public long? CreatedAt { get; set; }
    }
