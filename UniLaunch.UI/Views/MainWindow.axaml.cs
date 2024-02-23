using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using UniLaunch.UI.Services;
using UniLaunch.UI.ViewModels;

namespace UniLaunch.UI.Views;

public partial class MainWindow : Window
{
    private bool _mouseDownForWindowMoving;
    private PointerPoint _originalPoint;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void InputElement_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!_mouseDownForWindowMoving)
        {
            return;
        }

        var currentPoint = e.GetCurrentPoint(this);
        Position = new PixelPoint(Position.X + (int)(currentPoint.Position.X - _originalPoint.Position.X),
            Position.Y + (int)(currentPoint.Position.Y - _originalPoint.Position.Y));
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Source is not Panel && e.Source is not Rectangle && e.Source is not TextBlock)
        {
            return;
        }

        if (WindowState is WindowState.Maximized or WindowState.FullScreen)
        {
            return;
        }

        _mouseDownForWindowMoving = true;
        _originalPoint = e.GetCurrentPoint(this);
    }

    private void InputElement_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _mouseDownForWindowMoving = false;
    }

    private bool HasUnsavedChanges()
    {
        var vm = DataContext as MainWindowViewModel;
        return vm?.HasUnsavedChanges == true;
    }

    private async void BeforeClose(object? sender, WindowClosingEventArgs e)
    {
        if (!HasUnsavedChanges())
        {
            return;
        }

        // Cancel the close in any case since we are in a sync context and method wont wait for dialog result
        e.Cancel = true;

        var result = await MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams()
        {
            Icon = MsBox.Avalonia.Enums.Icon.Warning,
            ButtonDefinitions = ButtonEnum.YesNo,
            ContentTitle = "You have unsaved changes",
            ContentMessage = "You have unsaved changes. Do you really want to exit?"
        }).ShowAsync();

        if (result == ButtonResult.Yes)
        {
            Environment.Exit(0);
        }
    }
}