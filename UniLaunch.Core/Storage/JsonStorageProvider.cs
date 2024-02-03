using Newtonsoft.Json.Serialization;
using UniLaunch.Core.Storage.JSON;
using UniLaunch.Core.Storage.Serialization;
using Newtonsoft.Json;

namespace UniLaunch.Core.Storage;

/// <summary>
/// StorageProvider for interacting with JSON files
/// </summary>
/// <typeparam name="T"></typeparam>
public class JsonStorageProvider<T> : StorageProvider<T>
{
    private readonly JsonSerializerSettings _jsonSerializerSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore,
        Converters = CustomTypeRegistry.TypeMapping
            .Select(mapping => new PropertyBasedConverter(
                mapping.Value.Property,
                mapping.Value.ValueTypeMapping,
                mapping.Key)
            )
            .ToArray()
            .Concat(new JsonConverter[]
            {
                new TimeOnlyConverter(),
                new WeekDayConverter()
            }).ToList()
    };

    public override string Extension => "json";

    public override void Persist(string filePathWithoutExtension, T data)
    {
        try
        {
            WriteFile(filePathWithoutExtension,
                JsonConvert.SerializeObject(data, Formatting.Indented, _jsonSerializerSettings));
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
            return JsonConvert.DeserializeObject<T>(GetFileContents(filePathWithOutExtension),
                _jsonSerializerSettings)!;
        }
        catch (Exception e)
        {
            throw new StorageException(e.Message, e);
        }
    }
}