using Newtonsoft.Json.Serialization;

namespace UniLaunch.Core.Storage;

using Newtonsoft.Json;
using System.IO;

public class JsonStorageProvider<T> : IStorageProvider<T>
{
    private readonly JsonSerializerSettings _jsonSerializerSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    public void Persist(string identifier, T data)
    {
        var json = JsonConvert.SerializeObject(data, Formatting.Indented, _jsonSerializerSettings);
        File.WriteAllText(identifier + ".json", json);
    }

    public T Load(string identifier)
    {
        if (!File.Exists(identifier + ".json"))
        {
            throw new StorageException("Could not find file");
        }
        
        var json = File.ReadAllText(identifier + ".json");
        return JsonConvert.DeserializeObject<T>(json, _jsonSerializerSettings)!;
    }
}
