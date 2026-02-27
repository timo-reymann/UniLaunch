using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using ReactiveUI;
using UniLaunch.Core.Meta;

namespace UniLaunch.UI.ViewModels;

public class AboutWindowViewModel : ViewModelBase
{
    private readonly AppInfoProvider _appInfoProvider;
    public ICommand OpenLink { get; }
    public ICommand OpenNoticeFile { get; }

    public AboutWindowViewModel()
    {
        _appInfoProvider = new AppInfoProvider();
        OpenLink = ReactiveCommand.Create<string>(_OpenLink);
        OpenNoticeFile = ReactiveCommand.Create(_OpenNoticeFile);
    }

    private void _OpenLink(string url)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }

    private void _OpenNoticeFile()
    {
        var noticeFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NOTICE.txt");
        if (!File.Exists(noticeFile))
            return;

        Process.Start(new ProcessStartInfo
        {
            FileName = noticeFile,
            UseShellExecute = true
        });
    }

    public FileVersionInfo VersionInfo => _appInfoProvider.VersionInfo;
}