using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace UniLaunch.Core.Spec;

/// <summary>
/// Classes implementing this interface are allowed to be named and their representation is directly displayable to users.
/// </summary>
public interface INameable
{
    public string Name { get; }
}