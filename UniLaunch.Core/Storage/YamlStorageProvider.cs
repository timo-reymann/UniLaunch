
using UniLaunch.Core.Storage.Serialization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using TimeOnlyConverter = UniLaunch.Core.Storage.YAML.TimeOnlyConverter;

namespace UniLaunch.Core.Storage;

/// <summary>
/// StorageProvider for interacting with YAML files
/// </summary>
/// <typeparam name="T"></typeparam>
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
    
    public YamlStorageProvider() {}

    public override string Extension => "yaml";

    public override void Persist(string filePathWithoutExtension, T data)
    {
        WriteFile(filePathWithoutExtension, yamlSerializer.Serialize(data));
    }

    public override T Load(string filePathWithOutExtension) => yamlDeserializer.Deserialize<T>(GetFileContents(filePathWithOutExtension));
}