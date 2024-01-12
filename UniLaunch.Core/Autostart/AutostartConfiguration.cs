using UniLaunch.Core.Rules;
using UniLaunch.Core.Targets;

namespace UniLaunch.Core.Autostart;

[Serializable]
public class AutostartConfiguration
{
    public List<RuleSet> RuleSets { get; set; } = new();
    public List<ITarget> Targets { get; set; } = new();
    public List<AutoStartEntry> Entries { get; set; } = new();

    public ITarget? GetTargetByName(string name) => Targets.FirstOrDefault(t => t?.Name == name);
    public RuleSet? GetRuleSetByName(string name) => RuleSets.FirstOrDefault(rs => rs.Name == name);
}