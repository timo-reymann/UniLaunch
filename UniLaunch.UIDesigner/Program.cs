using Avalonia;
using UniLaunch.UI;

namespace UniLaunch.UIEditor;

/// <summary>
/// This is a simple wrapper to allow the Avalonia UI designer to do its thing.
/// </summary>
internal static class Program
{
    public static void Main(string[] args) => EditorUi.Main(args);
    
    public static AppBuilder BuildAvaloniaApp() => EditorUi.BuildAvaloniaApp();
}