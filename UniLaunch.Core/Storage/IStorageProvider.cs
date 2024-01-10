namespace UniLaunch.Core.Storage;

public interface IStorageProvider<T>
{
    /// <summary>
    /// Persist data to storage
    /// </summary>
    /// <param name="identifier">Identifier for data to persist</param>
    /// <param name="data">Data to persist</param>
    public void Persist(string identifier, T data);

    /// <summary>
    /// Load data from storage
    /// </summary>
    /// <param name="identifier">Identifier for data to load</param>
    /// <returns>Deserialized data from storage</returns>
    public T Load(string identifier);
}
