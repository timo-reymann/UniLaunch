using UniLaunch.Core.Rules;
using UniLaunch.Core.Targets;

namespace UniLaunch.Core.Autostart;

[Serializable]
public record AutoStartEntry(
    string RuleSetName,
    string TargetName
);