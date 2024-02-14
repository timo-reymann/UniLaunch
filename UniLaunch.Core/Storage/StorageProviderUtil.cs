using UniLaunch.Core.Autostart;

namespace UniLaunch.Core.Storage;

public static class StorageProviderUtil
{
    public static string RemoveExtensionForPath(Uri fileUri, string extensionWithDot)
        => fileUri.AbsolutePath[..^(extensionWithDot.Length)];

    public static StorageProvider<UniLaunchConfiguration>? GetProviderForFileExtension(
        this List<StorageProvider<UniLaunchConfiguration>> storageProviders, string extensionWithDot)
    {
        return storageProviders
            .FirstOrDefault(s => s.Extension == extensionWithDot[1..]);
    }
}