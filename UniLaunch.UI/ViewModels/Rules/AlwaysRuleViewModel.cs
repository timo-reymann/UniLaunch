using UniLaunch.Core.Rules;
using UniLaunch.UI.CodeGeneration;
using UniLaunch.UI.Controls.Rules;

namespace UniLaunch.UI.ViewModels.Rules;

[GenerateViewModel(typeof(AlwaysRule), typeof(AlwaysRuleControl))]
public partial class AlwaysRuleViewModel
{
}