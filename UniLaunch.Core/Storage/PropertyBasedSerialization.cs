namespace UniLaunch.Core.Storage;

[AttributeUsage(AttributeTargets.Class)]
public class PropertyBasedSerialization : Attribute
{
    public PropertyBasedSerialization(string propertyName)
    {
        PropertyName = propertyName;
    }

    public string PropertyName { get; set; }
}