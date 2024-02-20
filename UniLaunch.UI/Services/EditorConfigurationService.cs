using System;
using System.IO;
using UniLaunch.Core.Storage;
using UniLaunch.UI.Configuration;

namespace UniLaunch.UI.Services;

public class EditorConfigurationService : IEditorConfigurationService
{
    private readonly string _settingsPath;
    private readonly StorageProvider<EditorConfiguration> _storageProvider;

    public EditorConfigurationService()
    {
        _settingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "UniLaunch",
            "appsettings"
        );
        _storageProvider = new JsonStorageProvider<EditorConfiguration>();

        Current = new EditorConfiguration();
    }

    public EditorConfiguration Current { get; private set; }

    public void Save()
    {
        _storageProvider.Persist(_settingsPath, Current);
    }

    public void LoadFromDisk()
    {
        Current = _storageProvider.Load(_settingsPath);
    }
}