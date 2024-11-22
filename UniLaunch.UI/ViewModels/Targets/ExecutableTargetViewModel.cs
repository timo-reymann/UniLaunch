using System.Linq;
using ReactiveUI;
using UniLaunch.Core.Targets;
using UniLaunch.UI.CodeGeneration;
using UniLaunch.UI.Controls.Targets;

namespace UniLaunch.UI.ViewModels.Targets;

[GenerateViewModel(typeof(ExecutableTarget), typeof(ExecutableTargetControl))]
public partial class ExecutableTargetViewModel
{
    public string ArgumentsListProperty
    {
        set
        {
            _model.Arguments = value.Split(" ");
            this.RaisePropertyChanged();
        }

        get => _model.Arguments != null ? string.Join(" ", _model.Arguments) : "";
    }
}