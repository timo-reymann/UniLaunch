namespace UniLaunch.Core.Storage.Serialization;

/// <summary>
/// Specify which property name should be used to extract the value for the type mapping
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class PropertyBasedSerializationAttribute : Attribute
{
    public PropertyBasedSerializationAttribute(string propertyName)
    {
        PropertyName = propertyName;
    }

    public string PropertyName { get; set; }
}