using System.Text.Json;
using System.Text.Json.Serialization;

namespace Maturix.Models;

/// <summary>
/// Updated RequestResponse class using JsonElement (simpler approach)
/// </summary>
internal class RequestResponse<T>
{
    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("statusmsg")]
    public string? StatusMessage { get; set; }

    [JsonPropertyName("data")]
    public JsonElement? Data { get; set; }

    /// <summary>
    /// Gets the typed data if it can be deserialized as T, otherwise returns null
    /// </summary>
    public T? GetTypedData(JsonSerializerOptions options)
    {
        if (Data == null || Data.Value.ValueKind == JsonValueKind.Null)
            return default;

        try
        {
            // Try to deserialize as T (handles objects and arrays)
            if (Data.Value.ValueKind == JsonValueKind.Object || Data.Value.ValueKind == JsonValueKind.Array)
            {
                return Data.Value.Deserialize<T>(options);
            }
        }
        catch (JsonException)
        {
            // Ignore deserialization errors
        }

        return default(T);
    }

    /// <summary>
    /// Gets error message from Data if it's a string, otherwise returns null
    /// </summary>
    public string? GetErrorMessage()
    {
        if (Data == null || Data.Value.ValueKind != JsonValueKind.String)
            return null;

        return Data.Value.GetString();
    }

    /// <summary>
    /// Checks if the data is a SuccessEnvelope and returns the Success value
    /// </summary>
    public int? GetSuccessCode(JsonSerializerOptions options)
    {
        var typedData = GetTypedData(options);
        if (typedData is SuccessEnvelope envelope)
            return envelope.Success;
        
        return null;
    }
}