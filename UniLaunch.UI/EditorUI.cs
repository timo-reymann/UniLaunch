﻿using Avalonia;
using Avalonia.ReactiveUI;
using System;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;
using UniLaunch.Core.Autostart;
using UniLaunch.Core.Rules;
using UniLaunch.Core.Targets;
using UniLaunch.UI.Configuration;
using UniLaunch.UI.ViewModels;
using UniLaunch.UI.ViewModels.Rules;
using ExecutableTargetViewModel = UniLaunch.UI.ViewModels.Targets.ExecutableTargetViewModel;

namespace UniLaunch.UI;

public static class EditorUi
{
    [STAThread]
    public static void Run(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        EntityViewModelRegistry.Instance
            .Register<EditorConfiguration, SettingsViewModel>()
            .Register<RuleSet, RulesetViewModel>()
            .Register<AutoStartEntry, AutoStartEntryViewModel>()
            .Register<AlwaysRule, AlwaysRuleViewModel>()
            .Register<TimeRule, TimeRuleViewModel>()
            .Register<WeekDayRule, WeekdayRuleViewModel>()
            .Register<ExecutableTarget, ExecutableTargetViewModel>();

        IconProvider.Current
            .Register<FontAwesomeIconProvider>();

        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    }
}