using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UniLaunch.Core.Storage.JSON;

/// <summary>
/// Converter for Newtonsoft.JSON to serialize based on a property names value to given types
/// </summary>
public class PropertyBasedConverter : JsonConverter
{
    public PropertyBasedConverter(string propertyName, Dictionary<string, Type> typeMapping, Type baseType)
    {
        TypeMapping = typeMapping;
        BaseType = baseType;
        PropertyName = propertyName;
    }

    protected Type BaseType { get; set; }

    protected Dictionary<string, Type> TypeMapping { get; }
    protected string PropertyName { get; }

    public override bool CanWrite => false;
    public override bool CanRead => true;

    public override bool CanConvert(Type objectType) => objectType == BaseType;
    public bool Accepts(Type type) => type == BaseType;

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