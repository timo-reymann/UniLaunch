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

    public override string Extension => "json";

    public override void Persist(string filePathWithoutExtension, T data)
    {
        WriteFile(filePathWithoutExtension, JsonConvert.SerializeObject(data, Formatting.Indented, _jsonSerializerSettings));
    }

    public override T Load(string filePathWithOutExtension) => 
        JsonConvert.DeserializeObject<T>(GetFileContents(filePathWithOutExtension), _jsonSerializerSettings)!;

}