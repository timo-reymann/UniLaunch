using Newtonsoft.Json;

namespace UniLaunch.Core.Storage.JSON;

/// <summary>
/// Convert time frames to and from JSON
/// </summary>
public class TimeOnlyConverter : JsonConverter
{
    private static readonly string Format = "HH:mm";

    public override bool CanConvert(Type objectType) => typeof(TimeOnly) == objectType;

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        writer.WriteValue(((TimeOnly)value).ToString(Format));
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        var value = reader.Value as string;
        if (TimeOnly.TryParseExact(value, Format, out var time))
        {
            return time;
        }

        throw new JsonSerializationException($"Invalid format for time '{value}', not in format {Format}");
    }
}