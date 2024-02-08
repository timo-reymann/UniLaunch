using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace UniLaunch.UI.Services;

public interface IFilesService
{
    public Task<IStorageFile?> OpenFileAsync(FilePickerOpenOptions? options = null);
    public Task<IStorageFolder?> OpenFolderAsync(FolderPickerOpenOptions? options = null);
    public Task<IStorageFile?> SaveFileAsync();
}