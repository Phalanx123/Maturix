using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Maturix.Models.Requests;

/// <summary>
/// Labels that can be assigned to a Production Plan
/// </summary>

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProductionPlanLabelEnum
{
    
    /// <summary>
    /// Blue 
    /// </summary>
    [EnumMember(Value ="blue")]
    Blue,
    /// <summary>
    /// Green
    /// </summary>
    [EnumMember(Value ="green")]
    Green,
    /// <summary>
    /// Orange
    /// </summary>
    [EnumMember(Value ="orange")]
    Orange,
    /// <summary>
    /// Red
    /// </summary>
    [EnumMember(Value ="red")]
    Red,
    /// <summary>
    /// Yellow
    /// </summary>
    [EnumMember(Value ="yellow")]
    Yellow,
    /// <summary>
    /// No Label
    /// </summary>
    [EnumMember(Value ="no_label")]
    NoLabel = 99
}