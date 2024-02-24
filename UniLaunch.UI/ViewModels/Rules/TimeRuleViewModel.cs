using ReactiveUI;
using UniLaunch.Core.Rules;
using UniLaunch.UI.CodeGeneration;
using UniLaunch.UI.Controls.Rules;

namespace UniLaunch.UI.ViewModels.Rules;

[GenerateViewModel(typeof(TimeRule), typeof(TimeRuleControl))]
public partial class TimeRuleViewModel
{
}