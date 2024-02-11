using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using Avalonia.Controls;
using DynamicData;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;
using ReactiveUI;
using UniLaunch.Core.Autostart;
using UniLaunch.Core.Meta;
using UniLaunch.Core.Rules;
using UniLaunch.Core.Spec;
using UniLaunch.Core.Storage;
using UniLaunch.UI.CodeGeneration;
using UniLaunch.UI.Services;
using Icon = MsBox.Avalonia.Enums.Icon;

namespace UniLaunch.UI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ICommand OpenFile { get; }
    public ICommand SaveFile { get; }
    public ICommand ShowAbout { get; }
    public ICommand Close { get; }
    public ICommand AddItem { get; }

    private int _selectedTab;
    private ObservableCollection<BaseEntityViewModel> _items = new();
    private BaseEntityViewModel? _selectedItem = null;

    public ObservableCollection<BaseEntityViewModel> Items
    {
        get => _items;
        set => this.RaiseAndSetIfChanged(ref _items, value);
    }

    public int SelectedTab
    {
        get => _selectedTab;
        set => this.RaiseAndSetIfChanged(ref _selectedTab, value);
    }

    public BaseEntityViewModel? SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    public MainWindowViewModel()
    {
        OpenFile = ReactiveCommand.Create(_OpenFile);
        ShowAbout = ReactiveCommand.Create(_ShowAbout);
        Close = ReactiveCommand.Create(_Close);
        SaveFile = ReactiveCommand.Create(_SaveFile);
        AddItem = ReactiveCommand.Create(_AddItem);

        this.WhenAnyValue(x => x.SelectedTab)
            .Subscribe(_ => SelectedTabChanged());
    }

    private void AddItemAndSelect(INameable model)
    {
        var viewModel = EntityViewModelRegistry.Instance.Of(model)!;
        Items.Add(viewModel);
        SelectedItem = viewModel;
    }

    private void _AddItem()
    {
        switch ((SelectedTabIndex)SelectedTab)
        {
            case SelectedTabIndex.Targets:
                // TODO Show target selector for MacOS or on other platforms use default
                break;

            case SelectedTabIndex.RuleSets:
                var ruleSet = new RuleSet()
                {
                    Name = $"New rule set ({Items.Count})"
                };
                UniLaunchEngine.Instance.Configuration!.RuleSets.Add(ruleSet);
                AddItemAndSelect(ruleSet);
                break;

            case SelectedTabIndex.Entries:
                var entry = new AutoStartEntry();
                UniLaunchEngine.Instance.Configuration!.Entries.Add(entry);
                AddItemAndSelect(entry);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateItems()
    {
        var config = UniLaunchEngine.Instance.Configuration!;
        var viewModelRegistry = EntityViewModelRegistry.Instance;

        Items.Clear();

        switch ((SelectedTabIndex)SelectedTab)
        {
            case SelectedTabIndex.Targets:
                Items.AddRange(viewModelRegistry.Of(config.Targets));
                break;

            case SelectedTabIndex.RuleSets:
                Items.AddRange(viewModelRegistry.Of(config.RuleSets));
                break;

            case SelectedTabIndex.Entries:
                Items.AddRange(viewModelRegistry.Of(config.Entries));
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(SelectedTab), SelectedTab, "Tab index not known");
        }
    }

    private void SelectedTabChanged()
    {
        UpdateItems();

        if (Items.Count > 0)
        {
            SelectedItem = Items[0];
        }
        else
        {
            SelectedItem = null;
        }
    }

    private void _Close()
    {
        Environment.Exit(0);
    }

    private async void _SaveFile()
    {
        try
        {
            UniLaunchEngine.Instance.PersistCurrentConfiguration();
        }
        catch (Exception e)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                "Failed to save file",
                $"Could not persist configuration: {e.Message}",
                ButtonEnum.Ok,
                Icon.Error
            ).ShowAsync();
        }
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

        var fileExtension = Path.GetExtension(file.Name);
        if (fileExtension.Length == 0)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                "Failed to parse file",
                $"{file.Name} could not be parsed: Files without extension in the file path are not supported by the editor.",
                ButtonEnum.Ok,
                Icon.Error
            ).ShowAsync();
            return;
        }

        StorageProvider<UniLaunchConfiguration>? storeProvider = null;
        foreach (var availableStoreProvider in UniLaunchEngine.Instance.AvailableStoreProviders)
        {
            if (fileExtension[1..] != availableStoreProvider.Extension)
            {
                continue;
            }

            storeProvider = availableStoreProvider;
            break;
        }

        if (storeProvider == null || fileExtension.Length == 0)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                "Failed to parse file",
                $"{file!.Name} could not be parsed: The file format is not supported.",
                ButtonEnum.Ok,
                Icon.Error
            ).ShowAsync();
            return;
        }

        try
        {
            var configFilePath = file.Path.AbsolutePath[..^(fileExtension.Length)];
            var config = storeProvider.Load(configFilePath);
            UniLaunchEngine.Instance.OverrideConfiguration(config, configFilePath, false);
            SelectedTabChanged();
        }
        catch (Exception e)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                "Failed to parse file",
                $"{file!.Name} could not be parsed: {e.Message}",
                ButtonEnum.Ok,
                Icon.Error
            ).ShowAsync();
        }
    }

    private enum SelectedTabIndex
    {
        Targets = 0,
        RuleSets = 1,
        Entries = 2,
    }
}