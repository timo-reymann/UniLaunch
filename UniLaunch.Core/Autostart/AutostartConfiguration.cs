using UniLaunch.Core.Rules;
using UniLaunch.Core.Targets;

namespace UniLaunch.Core.Autostart;

[Serializable]
public class AutostartConfiguration
{
    public List<RuleSet> RuleSets { get; set; } = new();
    public List<ITarget> Targets { get; set; } = new();
    public List<AutoStartEntry> Entries { get; set; } = new();
}