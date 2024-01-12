namespace UniLaunch.Core.Autostart;

[Serializable]
public record AutoStartEntry(
    string RuleSetName,
    string TargetName
);