using Newtonsoft.Json;
using UniLaunch.Core.Spec;
using YamlDotNet.Serialization;

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

    
    [JsonIgnore]
    [YamlIgnore]
    public string Name => $"{TargetName} when {RuleSetName} matches";
}