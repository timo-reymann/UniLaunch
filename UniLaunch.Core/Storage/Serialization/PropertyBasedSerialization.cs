namespace UniLaunch.Core.Storage.Serialization;

/// <summary>
/// Specify which property name should be used to extract the value for the type mapping
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class PropertyBasedSerialization : Attribute
{
    public PropertyBasedSerialization(string propertyName)
    {
        PropertyName = propertyName;
    }

    public string PropertyName { get; set; }
}