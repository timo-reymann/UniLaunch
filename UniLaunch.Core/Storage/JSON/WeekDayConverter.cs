using Newtonsoft.Json;

namespace UniLaunch.Core.Storage.JSON;

public class WeekDayConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => typeof(DayOfWeek) == objectType;

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        writer.WriteValue(((DayOfWeek)value).ToString("G"));
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        if (Enum.TryParse<DayOfWeek>(reader.Value as string, out var dayOfWeek))
        {
            return dayOfWeek;
        }

        throw new JsonSerializationException($"Could not parse weekday '{reader.Value}'");
    }
}