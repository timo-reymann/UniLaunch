namespace UniLaunch.Core.Storage.YAML;

using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

/// <summary>
/// Convert time frames from and to YAML
/// </summary>
public class TimeOnlyConverter : IYamlTypeConverter
{
    private static readonly string Format = "HH:mm";

    public bool Accepts(Type type)
    {
        return type == typeof(TimeOnly);
    }

    public object ReadYaml(IParser parser, Type type)
    {
        var scalar = parser.Consume<Scalar>();
        if (TimeOnly.TryParseExact(scalar.Value, Format, out var time))
        {
            return time;
        }

        throw new YamlException($"Invalid TimeOnly format: {scalar.Value}. Expected format: {Format}");
    }

    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
        if (value is TimeOnly time)
        {
            emitter.Emit(new Scalar(time.ToString(Format)));
        }
        else
        {
            throw new ArgumentException($"Expected value of type TimeOnly, but got {value.GetType().Name}");
        }
    }
}