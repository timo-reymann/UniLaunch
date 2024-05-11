using System.Diagnostics;
using System.Windows.Input;
using ReactiveUI;
using UniLaunch.Core.UpdateCheck;

namespace UniLaunch.UI.ViewModels;

public class UpdateNotificationWindowViewModel : ViewModelBase
{
    public AvailableUpdate AvailableUpdate { get; set; }
    public ICommand OpenLink { get; }
    
    public UpdateNotificationWindowViewModel()
    {
        OpenLink = ReactiveCommand.Create(_OpenLink);
    }

    private void _OpenLink()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = AvailableUpdate.DownloadPage.ToString(), 
            UseShellExecute = true
        });
    }
}