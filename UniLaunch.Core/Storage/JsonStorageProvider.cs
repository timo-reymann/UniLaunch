using Newtonsoft.Json.Serialization;
using UniLaunch.Core.Storage.JSON;
using UniLaunch.Core.Storage.Serialization;

namespace UniLaunch.Core.Storage;

using Newtonsoft.Json;
using System.IO;

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

    public override void Persist(string identifier, T data)
    {
        WriteFile(identifier, JsonConvert.SerializeObject(data, Formatting.Indented, _jsonSerializerSettings));
    }

    public override T Load(string identifier) => 
        JsonConvert.DeserializeObject<T>(GetFileContents(identifier), _jsonSerializerSettings)!;

    protected override string GetFilePath(string identifier) => CreateFilePath(identifier, "json");
}