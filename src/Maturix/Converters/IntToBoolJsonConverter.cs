using System.Text.Json;
using System.Text.Json.Serialization;

namespace Maturix.Converters;

/// <summary>
/// Converts between integer 0/1 and boolean true/false.
/// Useful when APIs return bool values as ints.
/// </summary>
public sealed class IntToBoolJsonConverter : JsonConverter<bool>
{
    /// <inheritdoc />
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Handle numbers: 0 = false, 1 = true
        if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetInt32(out var value))
                return value == 1;
        }

        // Handle JSON "true"/"false" strings or bool tokens
        if (reader.TokenType == JsonTokenType.True)
            return true;
        if (reader.TokenType == JsonTokenType.False)
            return false;

        if (reader.TokenType == JsonTokenType.String)
        {
            var s = reader.GetString();
            if (string.Equals(s, "1", StringComparison.OrdinalIgnoreCase))
                return true;
            if (string.Equals(s, "0", StringComparison.OrdinalIgnoreCase))
                return false;
            if (bool.TryParse(s, out var result))
                return result;
        }

        throw new JsonException($"Unexpected token {reader.TokenType} when parsing boolean.");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        // Always write as JSON true/false (not 0/1)
        writer.WriteBooleanValue(value);
    }
}