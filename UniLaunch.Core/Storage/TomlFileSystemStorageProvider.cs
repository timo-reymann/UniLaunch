using Tomlyn;
using Tomlyn.Syntax;

namespace UniLaunch.Core.Storage;

public class TomlFileSystemStorageProvider<T> : IStorageProvider<T> where T : class, new()
{
    private string Path { get; }

    public TomlFileSystemStorageProvider(string path)
    {
        Path = path;
    }

    private string GetJoinedPath(string identifier)
    {
        return System.IO.Path.Combine(Path, $"{identifier}.toml");
    }

    /// <inheritdoc />
    public void Persist(string identifier, T data)
    {
        if (!Toml.TryFromModel(data, out var serialized, out var errors))
        {
            throw new StorageException($"Could not serialize data to TOML: {errors}");
        }

        using var writer = new StreamWriter(GetJoinedPath(identifier));
        writer.Write(serialized);
    }

    /// <inheritdoc />
    public T Load(string identifier)
    {
        var filePath = GetJoinedPath(identifier);
        using var reader = new StreamReader(filePath);
        var serialized = reader.ReadToEnd();
        if (!Toml.TryToModel(serialized, out T? model, out DiagnosticsBag? errors, filePath))
        {
            throw new StorageException($"Could not deserialize data from TOML: {errors}");
        }

        return model;
    }
}