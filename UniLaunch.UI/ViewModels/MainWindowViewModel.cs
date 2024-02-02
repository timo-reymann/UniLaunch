using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Input;
using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using ReactiveUI;
using UniLaunch.Core.Meta;
using UniLaunch.UI.Services;
using Icon = MsBox.Avalonia.Enums.Icon;

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
        var provider = new AppInfoProvider();
        
        await MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams()
        {
            ButtonDefinitions = new List<ButtonDefinition>(),
            Markdown = true,
            ShowInCenter = true,
            ContentTitle = "About",
            SizeToContent = SizeToContent.WidthAndHeight,
            Icon = Icon.Info,
            ContentMessage = $"""
                             **Version:** {provider.VersionInfo.ProductVersion} 
                             
                             **Created by** Timo Reymann
                             
                             **GitHub**: timo-reymann/UniLaunch
                             """,
        }).ShowAsync();
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