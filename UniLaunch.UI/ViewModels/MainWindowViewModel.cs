using System;
using System.Collections.ObjectModel;
using DynamicData.Binding;
using ReactiveUI;
using UniLaunch.Core.Autostart;
using UniLaunch.Core.Spec;
using UniLaunch.UI.CodeGeneration;
using UniLaunch.UI.Services;

namespace UniLaunch.UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private int _selectedTab;
    private ObservableCollection<BaseEntityViewModel> _items = new();
    private BaseEntityViewModel? _selectedItem;
    private bool _buttonFlyoutVisible = false;
    private bool _hasUnsavedChanges = false;

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

    public bool HasUnsavedChanges
    {
        get => _hasUnsavedChanges;
        set => this.RaiseAndSetIfChanged(ref _hasUnsavedChanges, value);
    }

    public MainWindowViewModel()
    {
        CreateCommands();

        this.WhenAnyValue(x => x.SelectedTab)
            .Subscribe(_ => SelectedTabChanged());

        this.WhenAnyValue(x => x.SelectedItem)
            .Subscribe(item =>
            {
                item?.WhenAnyPropertyChanged(item.PropertiesToWatchForChanges)
                    .Subscribe(_ => HasUnsavedChanges = true);
            });
    }

    private void AddItemAndSelect(INameable model)
    {
        var viewModel = model.ToViewModel();
        Items.Add(viewModel);
        SelectedItem = viewModel;
        HasUnsavedChanges = true;
    }

    private static IFilesService GetFilesService()
    {
        var filesService = App.Current?.GetService<IFilesService>();
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