using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using UniLaunch.Core.Storage;
using UniLaunch.Core.UpdateCheck;
using UniLaunch.UI.Services;
using UniLaunch.UI.ViewModels;
using UniLaunch.UI.Views;

namespace UniLaunch.UI;

public class App : Application
{
    private DispatcherTimer? _confirmationTimer;

    /// <summary>
    /// Gets or sets the duration the tray icon is shown in autostart confirmation mode,
    /// or null when running the regular editor UI.
    /// </summary>
    public static TimeSpan? ConfirmationDuration { get; set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var editorConfigurationService = new EditorConfigurationService();

        AdjustEditorUIBasedOnSettings(editorConfigurationService);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };

            Services = new ServiceCollection()
                .AddSingleton<IFilesService>(_ => new FilesService(desktop.MainWindow))
                .AddSingleton<IWindowService>(_ => new WindowService(desktop.MainWindow))
                .AddSingleton<IEditorConfigurationService>(_ => editorConfigurationService)
                .BuildServiceProvider();

            if (TrayIcon.GetIcons(this)?.FirstOrDefault() is { } trayIcon)
            {
                trayIcon.ToolTipText = $"UniLaunch v{AppVersion.Current}";
            }

            if (ConfirmationDuration is { } duration)
            {
                desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                _confirmationTimer = new DispatcherTimer
                {
                    Interval = duration
                };
                _confirmationTimer.Tick += (_, _) => OnConfirmationElapsed(desktop);
                _confirmationTimer.Start();
            }
            else if (editorConfigurationService.Current.CheckForUpdatesOnLaunch)
            {
#pragma warning disable CS4014
                CheckForUpdates();
#pragma warning restore CS4014
            }
        }

        base.OnFrameworkInitializationCompleted();
    }

    internal void AdjustEditorUIBasedOnSettings(IEditorConfigurationService? editorConfigService = null)
    {
        editorConfigService ??= this.GetService<IEditorConfigurationService>()!;

        try
        {
            editorConfigService.LoadFromDisk();
        }
        catch (StorageException exc)
        {
            Console.WriteLine($"Warning: Could not load user config: {exc.Message}");
        }

        Assets.Resources.Culture = new CultureInfo(editorConfigService.Current.Language);
        Thread.CurrentThread.CurrentUICulture = Assets.Resources.Culture;

        RequestedThemeVariant = editorConfigService.Current.ThemeVariant switch
        {
            "Light" => ThemeVariant.Light,
            "Dark" => ThemeVariant.Dark,
            _ => ThemeVariant.Default
        };
    }

    private async Task CheckForUpdates()
    {
        var updateAvailableCheck = await new AvailableUpdateChecker().CheckAvailableUpdate();
        if (!updateAvailableCheck.Item1)
        {
            return;
        }

        await Services!
            .GetService<IWindowService>()!
            .ShowWindowAsDialog(new UpdateNotificationWindow
            {
                DataContext = new UpdateNotificationWindowViewModel
                {
                    AvailableUpdate = updateAvailableCheck.Item2!,
                }
            });
    }

    private void OnConfirmationElapsed(IClassicDesktopStyleApplicationLifetime desktop)
    {
        _confirmationTimer?.Stop();
        _confirmationTimer = null;
        desktop.Shutdown();
    }

    private void OnTrayClicked(object? sender, EventArgs e) => ShowMainWindow();

    private void OnOpen(object? sender, EventArgs e) => ShowMainWindow();

    private void OnExit(object? sender, EventArgs e)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }

    private void ShowMainWindow()
    {
        _confirmationTimer?.Stop();
        _confirmationTimer = null;

        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow is not { } window)
        {
            return;
        }

        // Restore close-is-exit behavior when coming from autostart confirmation mode.
        desktop.ShutdownMode = ShutdownMode.OnLastWindowClose;

        window.Show();
        if (window.WindowState == WindowState.Minimized)
        {
            window.WindowState = WindowState.Normal;
        }

        window.Activate();
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