using System.Text.Json;
using System.Text.Json.Serialization;

public class LanguagesConverter : JsonConverter<IReadOnlyDictionary<string, string>>
{
    public override IReadOnlyDictionary<string, string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dictionary = new Dictionary<string, string>();

        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            foreach (var item in doc.RootElement.EnumerateArray())
            {
                foreach (var property in item.EnumerateObject())
                {
                    dictionary[property.Name] = property.Value.GetString() ?? string.Empty;
                }
            }
        }

        return dictionary;
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyDictionary<string, string> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var kvp in value)
        {
            writer.WriteStartObject();
            writer.WriteString(kvp.Key, kvp.Value);
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
    }
}
