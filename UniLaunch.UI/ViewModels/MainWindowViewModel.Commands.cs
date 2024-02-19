using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using DynamicData;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;
using ReactiveUI;
using UniLaunch.Core.Autostart;
using UniLaunch.Core.Meta;
using UniLaunch.Core.Rules;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Targets;
using UniLaunch.UI.CodeGeneration;
using UniLaunch.UI.Services;
using UniLaunch.UI.Util;
using UniLaunch.UI.Views;

namespace UniLaunch.UI.ViewModels;

public partial class MainWindowViewModel
{
    public ICommand OpenFile { get; private set; } = null!;
    public ICommand SaveFile { get; private set; } = null!;
    public ICommand ShowAbout { get; private set; } = null!;
    public ICommand Close { get; private set; } = null!;
    public ICommand AddItem { get; private set; } = null!;
    public ICommand DeleteItem { get; private set; } = null!;

    private void CreateCommands()
    {
        OpenFile = ReactiveCommand.Create(_OpenFile);
        ShowAbout = ReactiveCommand.Create(_ShowAbout);
        Close = ReactiveCommand.Create(_Close);
        SaveFile = ReactiveCommand.Create<bool>(_SaveFile);
        AddItem = ReactiveCommand.Create<Type>(_AddItem);
        DeleteItem = ReactiveCommand.Create<BaseEntityViewModel>(_DeleteItem);
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

    private async void _ShowAbout()
    {
        (App.Current
                ?.Services?
                .GetService(typeof(IWindowService)) as WindowService)
            !.ShowWindowAsDialog(new AboutWindow
            {
                DataContext = new AboutWindowViewModel(),
            });
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

    private void _DeleteItem(BaseEntityViewModel viewModel)
    {
        var model = viewModel.Model;

        switch (IndexOfSelectedTab)
        {
            case SelectedTabIndex.Targets:
                Engine.Configuration!.Targets.Remove((Target)model);
                break;

            case SelectedTabIndex.RuleSets:
                Engine.Configuration!.RuleSets.Remove((RuleSet)model);
                break;

            case SelectedTabIndex.Entries:
                Engine.Configuration!.Entries.Remove((AutoStartEntry)model);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        var index = Items.IndexOf(viewModel);
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
}