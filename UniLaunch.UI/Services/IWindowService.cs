using Avalonia.Controls;

namespace UniLaunch.UI.Services;

public interface IWindowService
{
    public void ShowWindow(Window window);
    public void ShowWindowAsDialog(Window window);
}