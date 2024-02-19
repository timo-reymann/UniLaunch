using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using UniLaunch.Core.Autostart;
using UniLaunch.Core.Rules;
using UniLaunch.Core.Targets;
using UniLaunch.UI.CodeGeneration;
using UniLaunch.UI.Controls;

namespace UniLaunch.UI.ViewModels;

[GenerateViewModel(typeof(AutoStartEntry), typeof(AutoStartEntryControl))]
public partial class AutoStartEntryViewModel
{
    public List<Target> AvailableTargets => UniLaunchEngine.Instance.Configuration?.Targets ?? new();
    public List<RuleSet> AvailableRuleSets => UniLaunchEngine.Instance.Configuration.RuleSets ?? new();

    partial void InitViewModel()
    {
        _propertiesToWatchForChanges = _propertiesToWatchForChanges.Append(nameof(SelectedRuleSet))
            .Append(nameof(SelectedTarget))
            .ToArray();
    }

    public RuleSet? SelectedRuleSet
    {
        set
        {
            _model.RuleSetName = value?.Name ?? "";
            this.RaisePropertyChanged();
        }

        get => AvailableRuleSets.FirstOrDefault(ruleSet => ruleSet.Name == RuleSetNameProperty);
    }

    public Target? SelectedTarget
    {
        set
        {
            _model.TargetName = value?.Name ?? "";
            this.RaisePropertyChanged();
        }

        get => AvailableTargets.FirstOrDefault(target => target.Name == TargetNameProperty);
    }
}