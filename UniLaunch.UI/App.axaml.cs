using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using UniLaunch.Core.Autostart;
using UniLaunch.UI.Services;
using UniLaunch.UI.ViewModels;
using UniLaunch.UI.Views;

namespace UniLaunch.UI;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };

            Services = new ServiceCollection()
                .AddSingleton<IFilesService>(_ => new FilesService(desktop.MainWindow))
                .AddSingleton<IWindowService>(_ => new WindowService(desktop.MainWindow))
                .AddSingleton<IEditorConfigurationService>(_ => new EditorConfigurationService())
                .BuildServiceProvider();
        }

        base.OnFrameworkInitializationCompleted();
    }

    public new static App? Current => Application.Current as App;

    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
    /// </summary>
    public IServiceProvider? Services { get; private set; }
}

public static class AppExtensions
{
    public static T? GetService<T>(this App app) where T : class
    {
        return app.Services?.GetService(typeof(T)) as T;
    }
}