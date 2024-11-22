using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace UniLaunch.Core.Storage.YAML;

public class UriConverter : IYamlTypeConverter
{
    public bool Accepts(Type type)
    {
        return type == typeof(Uri);
    }

    public object? ReadYaml(IParser parser, Type type)
    {
        var scalar = parser.Consume<Scalar>();

        if (Uri.TryCreate(scalar.Value, new UriCreationOptions() { }, out var uri))
        {
            return uri;
        }

        throw new YamlException($"Invalid URI: {scalar.Value}");
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type)
    {
        if (value is Uri uri)
        {
            emitter.Emit(new Scalar(uri.ToString()));
        }
        else
        {
            throw new ArgumentException($"Expected value of type Uri, but got {value?.GetType().Name}");
        }
    }
}