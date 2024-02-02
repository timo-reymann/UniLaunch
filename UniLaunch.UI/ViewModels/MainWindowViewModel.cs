using System;
using System.Windows.Input;
using MsBox.Avalonia;
using ReactiveUI;
using UniLaunch.UI.Services;

namespace UniLaunch.UI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ICommand OpenFile { get; }
    public ICommand ShowAbout { get; }
    public ICommand Close { get; }

    private int _selectedTab;

    public int SelectedTab
    {
        get => _selectedTab;
        set => this.RaiseAndSetIfChanged(ref _selectedTab, value);
    }

    public MainWindowViewModel()
    {
        OpenFile = ReactiveCommand.Create(_OpenFile);
        ShowAbout = ReactiveCommand.Create(_ShowAbout);
        Close = ReactiveCommand.Create(_Close);
        this.ObservableForProperty(it => it.SelectedTab)
            .Subscribe(selectedTab => SelectedTabChanged());
    }

    private void SelectedTabChanged()
    {
        var tabs = new[]
        {
            "Targets",
            "Rulesets",
            "Entries"
        };
        Console.WriteLine($"Selected tab {tabs[SelectedTab]}");
        // TODO Add logic for different item load
    }

    private void _Close()
    {
        Environment.Exit(0);
    }

    private async void _ShowAbout()
    {
    }

    private async void _OpenFile()
    {
        var filesService = App.Current?.Services?.GetService(typeof(IFilesService)) as IFilesService;
        if (filesService is null)
        {
            throw new NullReferenceException("Missing File Service instance.");
        }

        var file = await filesService.OpenFileAsync();
        if (file == null)
        {
            return;
        }

        var box = MessageBoxManager
            .GetMessageBoxStandard("File selected", $"You selected {file!.Name}");
        await box.ShowAsync();
    }
}