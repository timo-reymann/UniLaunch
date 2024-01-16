namespace UniLaunch.Core.Autostart;

[Serializable]
public class AutoStartEntry
{
    public AutoStartEntry(string ruleSetName, string targetName)
    {
        this.RuleSetName = ruleSetName;
        this.TargetName = targetName;
    }
    
    public AutoStartEntry() {}

    public string RuleSetName { get; set; }
    public string TargetName { get; set; }

}