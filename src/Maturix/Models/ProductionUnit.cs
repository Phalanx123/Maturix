using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Maturix.Models;

/// <summary>
/// Represents an envelope for Quality Reports returned by the Maturix API.
/// </summary>
public class ProductionUnit
{
    [JsonPropertyName("Stats")]
    public ProductionStats? Stats { get; set; }
    
    [JsonPropertyName("Sensordata")]
    public List<ProductionSensorPoint>? SensorData { get; set; }
    
    
}