using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using UniLaunch.UI.CodeGeneration;
using UniLaunch.UI.ViewModels;

namespace UniLaunch.UI;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
        {
            return null;
        }

        var type = data is IAssociatedUserControl userControl
            ? userControl.UserControl
            : GetViewTypeForViewModel(data);

        if (type == null)
        {
            return new TextBlock { Text = $"Could not map view model {data.GetType().FullName} to view" };
        }

        var control = (Control)Activator.CreateInstance(type)!;
        control.DataContext = data;
        return control;
    }

    private static Type? GetViewTypeForViewModel(object data)
    {
        var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        return Type.GetType(name);
    }

    public bool Match(object? data)
    {
        return data is BaseEntityViewModel;
    }
}