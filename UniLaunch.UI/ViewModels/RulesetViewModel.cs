using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;
using UniLaunch.Core.Autostart;
using UniLaunch.Core.Rules;
using UniLaunch.UI.CodeGeneration;
using UniLaunch.UI.Controls;

namespace UniLaunch.UI.ViewModels;

[GenerateViewModel(typeof(RuleSet), typeof(RuleSetControl))]
public partial class RulesetViewModel
{
    public ImmutableHashSet<Type> EnabledRuleTypes => UniLaunchEngine.Instance.EnabledRuleTypes;
    public ICommand AddRule { get; private set; }

    public ObservableCollection<BaseEntityViewModel> RulesListProperty
    {
        get => new(EntityViewModelRegistry.Instance.Of(_model.Rules));
        set
        {
            _model.Rules = value.Select(m => (Rule)m.Model).ToList();
            this.RaisePropertyChanged();
        }
    }

    partial void InitViewModel()
    {
        AddRule = ReactiveCommand.Create<Type>(_AddRule);
    }

    private void _AddRule(Type t)
    {
        var rule = Activator.CreateInstance(t) as Rule;
        RulesListProperty.Add(rule!.ToViewModel()!);
        _model.Rules.Add(rule!);
        this.RaisePropertyChanged(nameof(RulesListProperty));
    }
}