namespace UniLaunch.Core.Storage;

[AttributeUsage(AttributeTargets.Class)]
public class PropertyValueForSerialization  : Attribute
{
    public PropertyValueForSerialization(string value)
    {
        Value = value;
    }

    public string Value { get; set; }
}