using System.Reflection;

namespace UniLaunch.Core.Storage.Serialization;

public record PropertyNameValueTypeMapping(
    string Property,
    PropertyValueTypeMapping ValueTypeMapping
);

public class PropertyValueTypeMapping : Dictionary<string, Type>
{
}

public static class CustomTypeRegistry
{
    public static void Register(Type type)
    {
        var baseType = type.BaseType!;

        var propertyName = type
            .GetCustomAttribute<PropertyBasedSerialization>(true)!
            .PropertyName;

        var propertyValue = type
            .GetCustomAttribute<PropertyValueForSerialization>(true)!
            .Value;

        if (
            !TypeMapping.TryAdd(
                baseType,
                new PropertyNameValueTypeMapping(propertyName, new PropertyValueTypeMapping()
                    { { propertyValue, type } }))
        )
        {
            TypeMapping[baseType].ValueTypeMapping.Add(propertyValue, type);
        }
    }

    public static readonly Dictionary<Type, PropertyNameValueTypeMapping> TypeMapping = new();
}