namespace UniLaunch.Core.Storage.Serialization;

/// <summary>
/// Property value to use to deserialize to the given type
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class PropertyValueForSerializationAttribute  : Attribute
{
    public PropertyValueForSerializationAttribute(string value)
    {
        Value = value;
    }

    public string Value { get; set; }
}