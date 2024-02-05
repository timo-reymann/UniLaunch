using ReactiveUI;
using UniLaunch.MacOS.Targets;
using UniLaunch.UI.CodeGeneration;

namespace UniLaunch.MacOS.ViewModel;

[GenerateViewModel(typeof(AppFileTarget))]
public partial class AppFileTargetViewModel : ReactiveObject
{
}