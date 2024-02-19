using System;
using System.Diagnostics;
using System.Windows.Input;
using ReactiveUI;
using UniLaunch.Core.Meta;

namespace UniLaunch.UI.ViewModels;

public class AboutWindowViewModel : ViewModelBase
{
    private readonly AppInfoProvider _appInfoProvider;
    public ICommand OpenLink { get; }

    public AboutWindowViewModel()
    {
        _appInfoProvider = new AppInfoProvider();
        OpenLink = ReactiveCommand.Create<string>(_OpenLink);
    }

    private void _OpenLink(string url)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = url, 
            UseShellExecute = true
        });
    }

    public FileVersionInfo VersionInfo => _appInfoProvider.VersionInfo;
}