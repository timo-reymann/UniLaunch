using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DynamicData.Binding;
using ReactiveUI;
using UniLaunch.Core.Autostart;
using UniLaunch.Core.Rules;
using UniLaunch.UI.CodeGeneration;
using UniLaunch.UI.Controls;

namespace UniLaunch.UI.ViewModels;

[GenerateViewModel(typeof(RuleSet), typeof(RuleSetControl))]
public partial class RulesetViewModel
{
    private ObservableCollection<BaseEntityViewModel>? _rules;
    public ImmutableHashSet<Type> EnabledRuleTypes => UniLaunchEngine.Instance.EnabledRuleTypes;
    public ICommand AddRule { get; private set; } = null!;
    public ICommand DeleteRule { get; private set; } = null!;

    public ObservableCollection<BaseEntityViewModel> RulesListProperty
    {
        get
        {
            if (_rules != null)
            {
                return _rules;
            }

            _rules = new(EntityViewModelRegistry.Instance.Of(_model.Rules));

            // register change listener for initial rules to trigger a change,
            // when new items are added the observable collection change event is enough to trigger it
            foreach (var rule in _rules)
            {
                rule.WhenAnyPropertyChanged(rule.PropertiesToWatchForChanges)
                    .Subscribe(_ => this.RaisePropertyChanged());
            }

            return _rules;
        }
        set
        {
            _model.Rules = value.Select(m => (Rule)m.Model).ToList();
            _rules = value;
            this.RaisePropertyChanged();
        }
    }

    partial void InitViewModel()
    {
        _propertiesToWatchForChanges = _propertiesToWatchForChanges
            .Append(nameof(RulesListProperty))
            .ToArray();
        AddRule = ReactiveCommand.Create<Type>(_AddRule);
        DeleteRule = ReactiveCommand.Create<BaseEntityViewModel>(_DeleteRule);
    }

    private void _AddRule(Type t)
    {
        var rule = Activator.CreateInstance(t) as Rule;
        RulesListProperty.Add(rule!.ToViewModel()!);
        _model.Rules.Add(rule!);
        this.RaisePropertyChanged(nameof(RulesListProperty));
    }

    private void _DeleteRule(BaseEntityViewModel vm)
    {
        var model = vm.Model;
        RulesListProperty.Remove(vm);
        _model.Rules.Remove(model as Rule);
        this.RaisePropertyChanged(nameof(RulesListProperty));
    }
}