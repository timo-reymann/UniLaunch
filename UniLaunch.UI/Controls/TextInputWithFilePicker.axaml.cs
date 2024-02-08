using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using UniLaunch.UI.Services;

namespace UniLaunch.UI.Controls;

public class TextInputWithFilePicker : TemplatedControl
{
    public static readonly DirectProperty<TextInputWithFilePicker, string> PathProperty =
        AvaloniaProperty.RegisterDirect<TextInputWithFilePicker, string>(
            "Path",
            o => o.Path,
            (o, v) => o.Path = v,
            defaultBindingMode: BindingMode.TwoWay);
    
    public static readonly DirectProperty<TextInputWithFilePicker, FilePickerOpenOptions> FilePickerOpenOptionsProperty =
        AvaloniaProperty.RegisterDirect<TextInputWithFilePicker, FilePickerOpenOptions>(
            "FilePickerOpenOptions",
            o => o.FilePickerOpenOptions,
            (o, v) => o.FilePickerOpenOptions = v,
            defaultBindingMode: BindingMode.TwoWay);

    private string _path = "";
    private FilePickerOpenOptions _filePickerOpenOptions;
    
    public string Path
    {
        get => _path;
        set => SetAndRaise(PathProperty, ref _path, value);
    }

    public FilePickerOpenOptions FilePickerOpenOptions
    {
        get => _filePickerOpenOptions;
        set => SetAndRaise(FilePickerOpenOptionsProperty, ref _filePickerOpenOptions, value);
    }

    private async void ClickHandler(object sender, RoutedEventArgs args)
    {
        var filesService = App.Current?.Services?.GetService(typeof(IFilesService)) as IFilesService;
        var file = await filesService.OpenFileAsync(FilePickerOpenOptions);
        if (file == null)
        {
            return;
        }
        
        Path = file.Path.AbsolutePath;
    }

    /// <summary>
    /// Called when the control's template is applied.
    /// In simple terms, this means the method is called just before the control is displayed.
    /// </summary>
    /// <param name="e">The event args.</param>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var btn = e.NameScope.Find("OpenFileBtn") as Button;
        btn!.Click += ClickHandler;
    }
}