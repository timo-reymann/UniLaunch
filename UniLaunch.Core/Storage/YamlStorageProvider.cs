using UniLaunch.Core.Storage.Serialization;
using UniLaunch.Core.Storage.YAML;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace UniLaunch.Core.Storage;

/// <summary>
/// StorageProvider for interacting with YAML files
/// </summary>
/// <typeparam name="T"></typeparam>
public class YamlStorageProvider<T> : StorageProvider<T>
{
    private readonly IDeserializer yamlDeserializer = new DeserializerBuilder()
        .WithTypeConverter(new TimeOnlyConverter())
        .WithTypeConverter(new UriConverter())
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
        .WithTypeConverter(new UriConverter())
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    public YamlStorageProvider()
    {
    }

    public override string Extension => "yml";

    public override void Persist(string filePathWithoutExtension, T data)
    {
        try
        {
            WriteFile(filePathWithoutExtension, yamlSerializer.Serialize(data));
        }
        catch (Exception e)
        {
            throw new StorageException(e.Message, e);
        }
    }

    public override T Load(string filePathWithOutExtension)
    {
        try
        {
            return yamlDeserializer.Deserialize<T>(GetFileContents(filePathWithOutExtension));
        }
        catch (Exception e)
        {
            throw new StorageException(e.Message, e);
        }
    }
}