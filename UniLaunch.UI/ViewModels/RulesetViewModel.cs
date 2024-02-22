using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;
using UniLaunch.Core.Rules;
using UniLaunch.UI.CodeGeneration;
using UniLaunch.UI.Controls;

namespace UniLaunch.UI.ViewModels;

[GenerateViewModel(typeof(RuleSet), typeof(RuleSetControl))]
public partial class RulesetViewModel
{
    public ObservableCollection<BaseEntityViewModel> RulesListProperty
    {
        get => new(EntityViewModelRegistry.Instance.Of(_model.Rules));
        set
        {
            _model.Rules = value.Select(m => (Rule)m.Model).ToList();
            this.RaisePropertyChanged();
        }
    }
}