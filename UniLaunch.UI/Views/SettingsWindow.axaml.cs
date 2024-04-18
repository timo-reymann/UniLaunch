using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using UniLaunch.Core.Storage;
using UniLaunch.UI.Services;
using UniLaunch.UI.ViewModels;

namespace UniLaunch.UI.Views;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
    }

    public bool HasConfigChangesForLoadedConfigFile { get; private set; }

    private async void OnClose(object? sender, WindowClosingEventArgs e)
    {
        HasConfigChangesForLoadedConfigFile =
            (DataContext as SettingsViewModel)!.HasChangesForConnectivityCheckConfig;
        
        if (e.IsProgrammatic)
        {
            return;
        }

        try
        {
            App.Current!
                .GetService<IEditorConfigurationService>()!
                .Save();
            App.Current!
                .AdjustEditorUIBasedOnSettings();
        }
        catch (StorageException exc)
        {
            e.Cancel = true;
            await MessageBoxManager.GetMessageBoxStandard(
                "Failed tos ave configuration",
                $"Failed to save: {exc.Message}",
                ButtonEnum.Ok,
                MsBox.Avalonia.Enums.Icon.Error
            ).ShowAsPopupAsync(this);
            Close();
        }
    }
}