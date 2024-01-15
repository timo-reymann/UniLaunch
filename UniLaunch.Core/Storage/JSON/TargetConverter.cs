using UniLaunch.Core.Targets;

namespace UniLaunch.Core.Storage.JSON;

public class TargetConverter : PropertyBasedConverter
{
    protected override string PropertyName => "configName";
    protected override Type TypeToSerialize { get; } = typeof(Target);

    protected override Dictionary<string, Type> TypeMapping { get; } = new()
    {
        { "executable", typeof(ExecutableTarget) }
    };
}