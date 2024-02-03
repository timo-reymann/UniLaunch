using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Controls;
using DynamicData;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using ReactiveUI;
using UniLaunch.Core.Autostart;
using UniLaunch.Core.Meta;
using UniLaunch.Core.Spec;
using UniLaunch.UI.Services;
using Icon = MsBox.Avalonia.Enums.Icon;

namespace UniLaunch.UI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ICommand OpenFile { get; }
    public ICommand ShowAbout { get; }
    public ICommand Close { get; }

    private int _selectedTab;
    private ObservableCollection<INameable> _items = new();
    private INameable _selectedItem = null!;

    public ObservableCollection<INameable> Items
    {
        get => _items;
        set => this.RaiseAndSetIfChanged(ref _items, value);
    }

    public int SelectedTab
    {
        get => _selectedTab;
        set => this.RaiseAndSetIfChanged(ref _selectedTab, value);
    }
    
    public INameable SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    public MainWindowViewModel()
    {
        OpenFile = ReactiveCommand.Create(_OpenFile);
        ShowAbout = ReactiveCommand.Create(_ShowAbout);
        Close = ReactiveCommand.Create(_Close);
        
        this.WhenAnyValue(x => x.SelectedTab)
            .Subscribe(_ => SelectedTabChanged());
    }

    private void SelectedTabChanged()
    {
        Items.Clear();
        var config = UniLaunchEngine.Instance.Configuration!;
        
        switch(SelectedTab)
        {
            case 0: // Targets
                Items.AddRange(config.Targets);
                break;
            
            case 1: // Rulesets
                Items.AddRange(config.RuleSets);
                break;
            
            case 2: // Entries
                Items.AddRange(config.Entries);
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(SelectedTab), SelectedTab, "Tab index not known");
        }
        Console.WriteLine(Items.Count);
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