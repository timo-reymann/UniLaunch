
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;
using TimeOnlyConverter = UniLaunch.Core.Storage.YAML.TimeOnlyConverter;

namespace UniLaunch.Core.Storage;

public class YamlStorageProvider<T> : StorageProvider<T>
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
                    .Invoke(o, new object[] { mapping.Value.Property, mapping.Value.ValueTypeMapping });
            }
        })
        .Build();

    private readonly ISerializer yamlSerializer = new SerializerBuilder()
        .WithTypeConverter(new TimeOnlyConverter())
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    public override void Persist(string identifier, T data)
    {
        WriteFile(identifier, yamlSerializer.Serialize(data));
    }

    protected override string GetFilePath(string identifier) => CreateFilePath(identifier, "yaml");

    public override T Load(string identifier) => yamlDeserializer.Deserialize<T>(GetFileContents(identifier));
}