using System.Text.Json;
using System.Text.Json.Serialization;

namespace CityPopulationWebService.JsonConverter;

public sealed class LongConverter : JsonConverter<long>
{
    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (long.TryParse(reader.GetString(), out long value))
            {
                return value;
            }
            else
            {
                return 0;
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            try
            {
                return reader.GetInt64();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        throw new JsonException($"Unexpected token type: {reader.TokenType}");
    }

    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}
