using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Maturix.Models;

/// <summary>
/// Represents an envelope for Quality Reports returned by the Maturix API.
/// </summary>
public class ProductionUnit
{
    /// <summary>
    /// Gets or sets the production stats.
    /// </summary>
    [JsonPropertyName("Stats")]
    public ProductionStats? Stats { get; set; }
    
    /// <summary>
    /// Data points from the sensor(s) associated with this production unit.
    /// </summary>
    [JsonPropertyName("Sensordata")]
    public List<ProductionSensorPoint>? SensorData { get; set; }
    
    
}