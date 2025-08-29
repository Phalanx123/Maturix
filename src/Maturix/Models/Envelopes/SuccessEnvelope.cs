using System.Text.Json.Serialization;
using OneOf;

namespace Maturix.Models.Envelopes;


/// <summary>
/// Represents the "data" payload from the Maturix API
/// which can either be a success object or a string error.
/// </summary>
public class SuccessData : OneOfBase<SuccessEnvelope, string>
{
    /// <inheritdoc />
    public SuccessData(OneOf<SuccessEnvelope, string> input) : base(input) { }
}

/// <summary>
/// Represents an envelope for success responses from the Maturix API.
/// </summary>
public class SuccessEnvelope
{
    /// <summary>
    /// Indicates whether the operation was successful (1 for success, 0 for failure).
    /// </summary>
    [JsonPropertyName("Success")]
    public int Success { get; set; }
}