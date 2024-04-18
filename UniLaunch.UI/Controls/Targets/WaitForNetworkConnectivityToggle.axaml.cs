using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace UniLaunch.UI.Controls.Targets;

public class WaitForNetworkConnectivityToggle : TemplatedControl
{
    public static readonly DirectProperty<WaitForNetworkConnectivityToggle, bool> StateProperty =
        AvaloniaProperty.RegisterDirect<WaitForNetworkConnectivityToggle, bool>(
            "State",
            o => o.State,
            (o, v) => o.State = v,
            defaultBindingMode: BindingMode.OneWay);

    private bool _state;

    public bool State
    {
        get => _state;
        set => SetAndRaise(StateProperty, ref _state, value);
    }
}