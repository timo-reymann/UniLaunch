using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace UniLaunch.Core.Storage.JSON;

public abstract class PropertyBasedConverter<T> : JsonConverter, IYamlTypeConverter
{
    protected abstract Dictionary<string, Type> TypeMapping { get; }
    protected abstract string PropertyName { get; }

    public override bool CanWrite => false;
    public override bool CanRead => true;

    public override bool CanConvert(Type objectType) => objectType == typeof(T);
    public bool Accepts(Type type) => type == typeof(T);

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


    public object? ReadYaml(IParser parser, Type type)
    {
        throw new NotImplementedException();
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type)
    {
        throw new NotImplementedException();
    }
}