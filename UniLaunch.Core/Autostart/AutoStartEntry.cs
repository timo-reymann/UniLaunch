using UniLaunch.Core.Rules;
using UniLaunch.Core.Spec;

namespace UniLaunch.Core.Autostart;

[Serializable]
public class AutoStartEntry : INameable
{
    public AutoStartEntry(string ruleSetName, string targetName)
    {
        RuleSetName = ruleSetName;
        TargetName = targetName;
    }
    
    public AutoStartEntry() {}

    public string RuleSetName { get; set; }
    public string TargetName { get; set; }

    public string Name => $"{TargetName} when {RuleSetName} matches";
}