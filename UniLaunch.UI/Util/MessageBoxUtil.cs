using System.Threading.Tasks;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace UniLaunch.UI.Util;

public static class MessageBoxUtil
{
    public static async Task ShowErrorDialog(string title, string text)
    {
        await MessageBoxManager.GetMessageBoxStandard(
            title,
            text,
            ButtonEnum.Ok,
            Icon.Error
        ).ShowAsync();
    }
}