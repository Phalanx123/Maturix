using System.Text.Json.Serialization;

namespace Maturix.Models;

/// <summary>
/// List of Sensors
/// </summary>
public class Sensor
{
    /// <summary>
    /// Human-readable identifier of the device.
    /// Example: "OLILMU"
    /// </summary>
    [JsonPropertyName("HRID")]
    public required string SensorId { get; set; }

    /// <summary>
    /// Optional name of the device.
    /// </summary>
    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    /// <summary>
    /// Indicates whether the device is currently in use.
    /// 0 = Not in use, 1 = In use.
    /// </summary>
    [JsonPropertyName("InUse")]
    public int InUse { get; set; }

    /// <summary>
    /// Battery percentage (0–100).
    /// </summary>
    [JsonPropertyName("Battery")]
    public int Battery { get; set; }

    /// <summary>
    /// Internal identifier (nullable).
    /// </summary>
    [JsonPropertyName("ID")]
    public string? Id { get; set; }

    /// <summary>
    /// Production-related identifier (nullable).
    /// </summary>
    [JsonPropertyName("ProductionID")]
    public string? ProductionId { get; set; }

    /// <summary>
    /// Workstation identifier where the device is assigned (nullable).
    /// </summary>
    [JsonPropertyName("Workstation")]
    public string? Workstation { get; set; }

    /// <summary>
    /// Default workstation ID.
    /// Typically, 0 if none is set.
    /// </summary>
    [JsonPropertyName("DefaultWorkstation")]
    public int DefaultWorkstation { get; set; }

    /// <summary>
    /// Template production identifier.
    /// Typically used for predefined production setups.
    /// </summary>
    [JsonPropertyName("TemplateProductionID")]
    public int TemplateProductionId { get; set; }

    /// <summary>
    /// Last heartbeat timestamp (epoch format).
    /// </summary>
    [JsonPropertyName("Heartbeat")]
    public long Heartbeat { get; set; }

    /// <summary>
    /// Encapsulates telemetry data such as temperature and voltage readings.
    /// </summary>
    [JsonPropertyName("Data")]
    public required DeviceData Data { get; set; }

    /// <summary>
    /// Transmission details (nullable).
    /// Could represent logs, messages, or sync status.
    /// </summary>
    [JsonPropertyName("Transmissions")]
    public string? Transmissions { get; set; }

    /// <summary>
    /// Link status or channel identifier.
    /// Example: 2
    /// </summary>
    [JsonPropertyName("Link")]
    public int Link { get; set; }
}

/// <summary>
/// Represents telemetry data captured by the device.
/// </summary>
public class DeviceData
{
    /// <summary>
    /// Epoch timestamp when the data was recorded.
    /// </summary>
    [JsonPropertyName("Timestamp")]
    public long Timestamp { get; set; }

    /// <summary>
    /// Temperature of the cable (°C).
    /// </summary>
    [JsonPropertyName("TempCable")]
    public double TempCable { get; set; }

    /// <summary>
    /// Ambient air temperature (°C).
    /// </summary>
    [JsonPropertyName("TempAir")]
    public double TempAir { get; set; }

    /// <summary>
    /// Voltage reading on the cable.
    /// </summary>
    [JsonPropertyName("VoltCable")]
    public double VoltCable { get; set; }

    /// <summary>
    /// Trigger value (0 = inactive, 1 = active).
    /// </summary>
    [JsonPropertyName("Trigger")]
    public int Trigger { get; set; }

    /// <summary>
    /// Battery voltage (V).
    /// Example: 2.83
    /// </summary>
    [JsonPropertyName("Battery")]
    public double Battery { get; set; }
}