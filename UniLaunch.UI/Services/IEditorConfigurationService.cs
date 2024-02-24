using UniLaunch.UI.Configuration;

namespace UniLaunch.UI.Services;

public interface IEditorConfigurationService
{
    public EditorConfiguration Current { get; }
    public void Save();
    public void LoadFromDisk();
}