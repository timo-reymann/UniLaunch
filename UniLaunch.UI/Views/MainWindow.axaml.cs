using System;
using Avalonia.Controls;

namespace UniLaunch.UI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        // only use native menu on macos
        if (OperatingSystem.IsMacOS())
        {
            InlineAppMenu.IsVisible = false;
        }
    }
}