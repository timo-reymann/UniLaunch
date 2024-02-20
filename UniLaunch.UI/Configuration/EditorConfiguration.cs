using UniLaunch.Core.Spec;

namespace UniLaunch.UI.Configuration;

public class EditorConfiguration : INameable
{
    public string Name => "Editor UI Configuration";

    public string ThemeVariant { get; set; } = "Default";
}