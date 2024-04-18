using System.Threading.Tasks;
using Avalonia.Controls;

namespace UniLaunch.UI.Services;

public interface IWindowService
{
    public void ShowWindow(Window window);
    public Task ShowWindowAsDialog(Window window);
}