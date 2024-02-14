using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using DynamicData;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using ReactiveUI;
using UniLaunch.Core.Autostart;
using UniLaunch.Core.Meta;
using UniLaunch.Core.Rules;
using UniLaunch.Core.Spec;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Targets;
using UniLaunch.UI.CodeGeneration;
using UniLaunch.UI.Services;
using UniLaunch.UI.Util;
using Icon = MsBox.Avalonia.Enums.Icon;

namespace UniLaunch.UI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ICommand OpenFile { get; }
    public ICommand SaveFile { get; }
    public ICommand ShowAbout { get; }
    public ICommand Close { get; }
    public ICommand AddItem { get; }
    public ICommand DeleteCurrentItem { get; }

    private int _selectedTab;
    private ObservableCollection<BaseEntityViewModel> _items = new();
    private BaseEntityViewModel? _selectedItem;
    private bool _buttonFlyoutVisible = false;

    public UniLaunchEngine Engine => UniLaunchEngine.Instance;

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

    private SelectedTabIndex IndexOfSelectedTab => (SelectedTabIndex)SelectedTab;

    public bool ButtonFlyoutVisible
    {
        get => _buttonFlyoutVisible;
        set => this.RaiseAndSetIfChanged(ref _buttonFlyoutVisible, value);
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
        SaveFile = ReactiveCommand.Create<bool>(_SaveFile);
        AddItem = ReactiveCommand.Create<Type>(_AddItem);
        DeleteCurrentItem = ReactiveCommand.Create(_DeleteCurrentItem);

        this.WhenAnyValue(x => x.SelectedTab)
            .Subscribe(_ => SelectedTabChanged());
    }

    private void _DeleteCurrentItem()
    {
        switch (IndexOfSelectedTab)
        {
            case SelectedTabIndex.Targets:
                Engine.Configuration!.Targets.Remove((Target)SelectedItem.Model);
                break;

            case SelectedTabIndex.RuleSets:
                Engine.Configuration!.RuleSets.Remove((RuleSet)SelectedItem.Model);
                break;

            case SelectedTabIndex.Entries:
                Engine.Configuration!.Entries.Remove((AutoStartEntry)SelectedItem.Model);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        var index = Items.IndexOf(SelectedItem);
        Items.RemoveAt(index);

        if (Items.Count == 0)
        {
            SelectedItem = null;
        }
        else if (index > 1)
        {
            SelectedItem = Items.Last();
        }
        else
        {
            SelectedItem = Items.First();
        }
    }

    private void AddItemAndSelect(INameable model)
    {
        var viewModel = model.ToViewModel();
        Items.Add(viewModel);
        SelectedItem = viewModel;
    }

    private void _AddItem(Type? t = null)
    {
        switch (IndexOfSelectedTab)
        {
            case SelectedTabIndex.Targets:
                var instance = Activator.CreateInstance(t ?? Engine.EnabledTargetTypes.First()) as Target;
                instance!.Name = $" {instance.TargetType} ({Items.Count})";
                Engine.Configuration!.Targets.Add(instance);
                AddItemAndSelect(instance);
                break;

            case SelectedTabIndex.RuleSets:
                var ruleSet = new RuleSet
                {
                    Name = $"New rule set ({Items.Count})"
                };
                Engine.Configuration!.RuleSets.Add(ruleSet);
                AddItemAndSelect(ruleSet);
                break;

            case SelectedTabIndex.Entries:
                var entry = new AutoStartEntry();
                Engine.Configuration!.Entries.Add(entry);
                AddItemAndSelect(entry);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateItems()
    {
        var config = Engine.Configuration!;

        Items.Clear();

        switch (IndexOfSelectedTab)
        {
            case SelectedTabIndex.Targets:
                Items.AddRange(config.Targets.ToViewModel());
                break;

            case SelectedTabIndex.RuleSets:
                Items.AddRange(config.RuleSets.ToViewModel());
                break;

            case SelectedTabIndex.Entries:
                Items.AddRange(config.Entries.ToViewModel());
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(SelectedTab), SelectedTab, "Tab index not known");
        }
    }

    private void SelectedTabChanged()
    {
        UpdateItems();
        SelectedItem = Items.Count > 0 ? Items[0] : null;
        ButtonFlyoutVisible = Engine.EnabledTargetTypes.Count > 1 &&
                              (SelectedTabIndex)SelectedTab == SelectedTabIndex.Targets;
    }

    private void _Close()
    {
        Environment.Exit(0);
    }

    private async void _SaveFile(bool withFilePicker = false)
    {
        if (withFilePicker)
        {
            var possibleExtensions = Engine.AvailableStoreProviders.GetAllExtensions();
            var file = await GetFilesService()!.SaveFileAsync(new FilePickerSaveOptions()
            {
                Title = "Select file to save to",
                FileTypeChoices = possibleExtensions.Select(extension =>
                        new FilePickerFileType($"UniLaunch {extension.ToUpper()}")
                        {
                            Patterns = [$"*.{extension}"]
                        })
                    .ToList()
            });

            if (file == null)
            {
                return;
            }

            var fileExtension = Path.GetExtension(file.Name);
            if (fileExtension.Length == 0 || !possibleExtensions.Contains(fileExtension[1..]))
            {
                await MessageBoxUtil.ShowErrorDialog(
                    "Failed to save file",
                    $"{file.Name} could not be used for saving: Invalid file type"
                );
                return;
            }

            var storageProvider = Engine.AvailableStoreProviders.GetProviderForFileExtension(fileExtension)!;
            Engine.DefaultStorageProvider = storageProvider;
            Engine.ConfigFilePath = StorageProviderUtil.RemoveExtensionForPath(file.Path, fileExtension);
        }

        try
        {
            Engine.PersistCurrentConfiguration();
        }
        catch (Exception e)
        {
            await MessageBoxUtil.ShowErrorDialog(
                "Failed to save file",
                $"Could not persist configuration: {e.Message}"
            );
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
        var file = await GetFilesService().OpenFileAsync();
        if (file == null)
        {
            return;
        }

        var fileExtension = Path.GetExtension(file.Name);
        if (fileExtension.Length == 0)
        {
            await MessageBoxUtil.ShowErrorDialog(
                "Failed to parse file",
                $"{file.Name} could not be parsed: Files without extension in the file path are not supported by the editor."
            );
            return;
        }

        var storageProvider = Engine.AvailableStoreProviders.GetProviderForFileExtension(fileExtension);
        if (storageProvider == null || fileExtension.Length == 0)
        {
            await MessageBoxUtil.ShowErrorDialog(
                "Failed to parse file",
                $"{file!.Name} could not be parsed: The file format is not supported."
            );
            return;
        }

        try
        {
            var configFilePath = StorageProviderUtil.RemoveExtensionForPath(file.Path, fileExtension);
            Engine.OverrideConfiguration(storageProvider.Load(configFilePath), configFilePath, false);
            SelectedTabChanged();
        }
        catch (Exception e)
        {
            await MessageBoxUtil.ShowErrorDialog(
                "Failed to parse file",
                $"{file!.Name} could not be parsed: {e.Message}"
            );
        }
    }

    private static IFilesService GetFilesService()
    {
        var filesService = App.Current?.Services?.GetService(typeof(IFilesService)) as IFilesService;
        if (filesService is null)
        {
            throw new NullReferenceException("Missing File Service instance.");
        }

        return filesService;
    }

    private enum SelectedTabIndex
    {
        Targets = 0,
        RuleSets = 1,
        Entries = 2,
    }
}