using System.Threading;
using UniLaunch.Core.Spec;

namespace UniLaunch.UI.Configuration;

public class EditorConfiguration : INameable
{
    public string Name => "Editor UI Configuration";
    public string ThemeVariant { get; set; } = "Default";
    public string Language { get; set; } = "en";
    public bool CheckForUpdatesOnLaunch { get; set; } = true;
}