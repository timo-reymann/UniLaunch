namespace UniLaunch.Core.Storage;

public abstract class StorageProvider<T>
{
    /// <summary>
    /// Persist data to storage
    /// </summary>
    /// <param name="identifier">Identifier for data to persist</param>
    /// <param name="data">Data to persist</param>
    public abstract void Persist(string identifier, T data);

    /// <summary>
    /// Load data from storage
    /// </summary>
    /// <param name="identifier">Identifier for data to load</param>
    /// <returns>Deserialized data from storage</returns>
    public abstract T Load(string identifier);

    protected abstract string GetFilePath(string identifier);

    protected void WriteFile(string identifier, string contents)
    {
        var fileName = GetFilePath(identifier);
        File.WriteAllText(fileName, contents);
    }

    protected string GetFileContents(string identifier)
    {
        var fileName = GetFilePath(identifier);

        if (!File.Exists(fileName))
        {
            throw new StorageException($"Could not find file {fileName}");
        }

        return File.ReadAllText(fileName);
    }

    protected string CreateFilePath(string identifier, string extension) => $"{identifier}.{extension}";
}