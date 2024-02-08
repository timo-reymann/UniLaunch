namespace UniLaunch.UI.Services;

using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

public class FilesService(Window target) : IFilesService
{
    public async Task<IStorageFile?> OpenFileAsync(FilePickerOpenOptions? options = null)
    {
        var files = await target.StorageProvider.OpenFilePickerAsync(options ?? new FilePickerOpenOptions());
        return files.Count >= 1 ? files[0] : null;
    }

    public async Task<IStorageFolder?> OpenFolderAsync(FolderPickerOpenOptions? options = null)
    {
        var folder = await target.StorageProvider.OpenFolderPickerAsync(options ?? new FolderPickerOpenOptions());
        return folder.Count >= 1 ? folder[0] : null;
    }

    public async Task<IStorageFile?> SaveFileAsync()
    {
        return await target.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {
            Title = "Save Text File"
        });
    }
}