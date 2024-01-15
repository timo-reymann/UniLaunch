using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UniLaunch.Core.Storage.JSON;

public abstract class PropertyBasedConverter : JsonConverter
{
    protected abstract Type TypeToSerialize { get; }
    protected abstract Dictionary<string, Type> TypeMapping { get; }
    protected abstract string PropertyName { get; }

    public override bool CanWrite => false;
    public override bool CanRead => true;

    public override bool CanConvert(Type objectType)
    {
        return objectType == TypeToSerialize;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new InvalidOperationException("Use default serialization.");
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var value = jsonObject[PropertyName].Value<string>();
        var rule = Activator.CreateInstance(TypeMapping[value]);

        serializer.Populate(jsonObject.CreateReader(), rule);
        return rule;
    }
}