using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace UniLaunch.UI.Controls;

public class TabItemWithIcon : TemplatedControl
{
    public static readonly DirectProperty<TabItemWithIcon, string> TitleProperty =
        AvaloniaProperty.RegisterDirect<TabItemWithIcon, string>(
            "Title",
            o => o.Title,
            (o, v) => o.Title = v,
            defaultBindingMode: BindingMode.OneWay);
    
    private string _title = null!;

    public string Title
    {
        get => _title;
        set => SetAndRaise(TitleProperty, ref _title, value);
    }
    
    public static readonly DirectProperty<TabItemWithIcon, string> IconProperty =
        AvaloniaProperty.RegisterDirect<TabItemWithIcon, string>(
            "Icon",
            o => o.Icon,
            (o, v) => o.Icon = v,
            defaultBindingMode: BindingMode.OneWay);
    
    private string _icon = null!;

    public string Icon
    {
        get => _icon;
        set => SetAndRaise(TitleProperty, ref _icon, value);
    }
}