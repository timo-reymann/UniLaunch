using UniLaunch.Core.Storage;
using UniLaunch.Core.Storage.Serialization;

namespace UniLaunch.Core.Rules;

/// <summary>
/// Always match
/// </summary>
[PropertyValueForSerialization("always")]
public class AlwaysRule : Rule
{
    public override bool Match(ExecutionContext context) => true;
    public override string RuleName => "always";
}