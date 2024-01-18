namespace UniLaunch.Core.Autostart;

[Serializable]
public class AutoStartEntry
{
    public AutoStartEntry(string ruleSetName, string targetName)
    {
        RuleSetName = ruleSetName;
        TargetName = targetName;
    }
    
    public AutoStartEntry() {}

    public string RuleSetName { get; set; }
    public string TargetName { get; set; }

}