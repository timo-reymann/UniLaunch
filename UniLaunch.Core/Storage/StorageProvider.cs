namespace UniLaunch.Core.Storage;

public abstract class StorageProvider<T>
{
    /// <summary>
    /// Extension for files written by the storage provider.
    ///
    /// The implementing StorageProvider sets this and only works with paths excluding the extension provided here.
    /// </summary>
    public abstract string Extension { get; }

    /// <summary>
    /// Persist data to storage
    /// </summary>
    /// <param name="filePathWithoutExtension">File path without extension</param>
    /// <param name="data">Data to persist</param>
    public abstract void Persist(string filePathWithoutExtension, T data);

    /// <summary>
    /// Load data from storage
    /// </summary>
    /// <param name="filePathWithOutExtension">File path without extension</param>
    /// <returns>Deserialized data from storage</returns>
    public abstract T Load(string filePathWithOutExtension);

    protected void WriteFile(string filePathWithoutExtension, string contents)
    {
        File.WriteAllText(AddExtension(filePathWithoutExtension), contents);
    }

    protected string AddExtension(string filePathWithoutExtension) => $"{filePathWithoutExtension}.{Extension}";

    protected string GetFileContents(string filePathWithoutExtension)
    {
        var filePath = AddExtension(filePathWithoutExtension);

        if (!File.Exists(filePath))
        {
            throw new StorageException($"Could not find file {filePath}");
        }

        return File.ReadAllText(filePath);
    }
}