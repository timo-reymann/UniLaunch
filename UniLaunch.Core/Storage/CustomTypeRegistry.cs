using UniLaunch.Core.Rules;
using UniLaunch.Core.Targets;

namespace UniLaunch.Core.Storage;

public record Mapping(
    string Property,
    Dictionary<string, Type> ValueMapping
);

public static class CustomTypeRegistry
{
    public static readonly Dictionary<Type, Mapping> TypeMapping = new()
    {
        {
            typeof(Target),
            new Mapping(
                "configName",
                new Dictionary<string, Type>
                {
                    { "executable", typeof(ExecutableTarget) },
                    { "appFile", typeof(AppFileTarget) }
                }
            )
        },
        {
            typeof(Rule),
            new Mapping(
                "ruleName",
                new Dictionary<string, Type>
                {
                    { "always", typeof(AlwaysRule) },
                    { "time", typeof(TimeRule) },
                    { "week-day", typeof(WeekDayRule) }
                }
            )
        }
    };
}