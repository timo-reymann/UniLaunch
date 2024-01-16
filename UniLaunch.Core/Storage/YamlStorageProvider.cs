
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;
using TimeOnlyConverter = UniLaunch.Core.Storage.YAML.TimeOnlyConverter;

namespace UniLaunch.Core.Storage;

public class YamlStorageProvider<T> : IStorageProvider<T>
{
    private readonly IDeserializer yamlDeserializer = new DeserializerBuilder()
        .WithTypeConverter(new YAML.TimeOnlyConverter())
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .WithTypeDiscriminatingNodeDeserializer((o) =>
        {
            var addKeyValueTypeDiscriminator = o.GetType()
                .GetMethod("AddKeyValueTypeDiscriminator")!;

            foreach (var mapping in CustomTypeRegistry.TypeMapping)
            {
                addKeyValueTypeDiscriminator
                    .MakeGenericMethod(mapping.Key)
                    .Invoke(o, new object[] { mapping.Value.Property, mapping.Value.ValueMapping });
            }
        })
        .Build();

    private readonly ISerializer yamlSerializer = new SerializerBuilder()
        .WithTypeConverter(new TimeOnlyConverter())
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    public void Persist(string identifier, T data)
    {
        string yaml = yamlSerializer.Serialize(data);
        File.WriteAllText(identifier + ".yaml", yaml);
    }

    public T Load(string identifier)
    {
        if (!File.Exists(identifier + ".yaml"))
        {
            return default(T);
        }

        string yaml = File.ReadAllText(identifier + ".yaml");
        return yamlDeserializer.Deserialize<T>(yaml);
    }
}