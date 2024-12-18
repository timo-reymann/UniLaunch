using System.Threading.Tasks;
using Avalonia.Controls;
using UniLaunch.UI.Views;

namespace UniLaunch.UI.Services;

public class WindowService(Window mainWindow) : IWindowService
{
    public void ShowWindow(Window window)
    {
        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        window.Show(mainWindow);
    }

    public Task ShowWindowAsDialog(Window window)
    {
        window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        return window.ShowDialog(mainWindow);
    }
}