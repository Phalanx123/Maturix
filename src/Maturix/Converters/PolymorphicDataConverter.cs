using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;

namespace Maturix.Converters;

/// <inheritdoc />
public class PolymorphicDataConverter<T> : JsonConverter<OneOf<T, string>?>
{
    /// <inheritdoc />
    public override OneOf<T, string>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;
            // Try to read as string first
            case JsonTokenType.String:
                return reader.GetString()!;
        }

        // Try to deserialize as the expected type T
        if (reader.TokenType != JsonTokenType.StartObject && reader.TokenType != JsonTokenType.StartArray) return null;
        try
        {
            var obj = JsonSerializer.Deserialize<T>(ref reader, options);
            if (obj != null)
                return obj;
        }
        catch (JsonException)
        {
            // If deserialization fails, fall through to return null
        }

        return null;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, OneOf<T, string>? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        value.Value.Switch(
            obj => JsonSerializer.Serialize(writer, obj, options),
            writer.WriteStringValue
        );
    }
}