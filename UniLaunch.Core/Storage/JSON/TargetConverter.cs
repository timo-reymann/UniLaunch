using UniLaunch.Core.Targets;

namespace UniLaunch.Core.Storage.JSON;

public class TargetConverter : PropertyBasedConverter<Target>
{
    protected override string PropertyName => "configName";

    protected override Dictionary<string, Type> TypeMapping { get; } = new()
    {
        { "executable", typeof(ExecutableTarget) },
        { "appFile", typeof(AppFileTarget) }
    };
}