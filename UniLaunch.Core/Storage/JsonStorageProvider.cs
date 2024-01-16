using Newtonsoft.Json.Serialization;
using UniLaunch.Core.Storage.JSON;

namespace UniLaunch.Core.Storage;

using Newtonsoft.Json;
using System.IO;

public class JsonStorageProvider<T> : IStorageProvider<T>
{
    private readonly JsonSerializerSettings _jsonSerializerSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore,
        Converters = CustomTypeRegistry.TypeMapping
            .Select(mapping => new PropertyBasedConverter(
                mapping.Value.Property,
                mapping.Value.ValueMapping,
                mapping.Key)
            )
            .ToArray()
            .Concat(new JsonConverter[]
            {
                new TimeOnlyConverter(),
                new WeekDayConverter()
            }).ToList()
    };

    private static string GetFileName(string identifier)
    {
        return $"{identifier}.json";
    }

    public void Persist(string identifier, T data)
    {
        var json = JsonConvert.SerializeObject(data, Formatting.Indented, _jsonSerializerSettings);
        File.WriteAllText(GetFileName(identifier), json);
    }

    public T Load(string identifier)
    {
        var fileName = GetFileName(identifier);

        if (!File.Exists(fileName))
        {
            throw new StorageException($"Could not find file {fileName}");
        }

        var json = File.ReadAllText(fileName);
        return JsonConvert.DeserializeObject<T>(json, _jsonSerializerSettings)!;
    }
}