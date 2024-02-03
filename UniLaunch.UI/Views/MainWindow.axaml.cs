using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using UniLaunch.Core.Autostart;

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
        if (e.Source is not Panel && e.Source is not Image)
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
}