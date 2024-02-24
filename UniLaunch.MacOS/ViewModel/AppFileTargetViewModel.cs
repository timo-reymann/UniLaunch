using Avalonia.Platform.Storage;
using UniLaunch.MacOS.Controls;
using UniLaunch.MacOS.Targets;
using UniLaunch.UI.CodeGeneration;

namespace UniLaunch.MacOS.ViewModel;

[GenerateViewModel(typeof(AppFileTarget), typeof(AppFileTargetControl))]
public partial class AppFileTargetViewModel
{
    public FilePickerOpenOptions FilePickerOpenOptions { get; } = new()
    {
        FileTypeFilter =
        [
            new("App-File")
            {
                AppleUniformTypeIdentifiers = ["applicationBundle"]
            }
        ]
    };
}