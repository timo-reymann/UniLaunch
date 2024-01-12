using System.Runtime.Serialization.Formatters.Binary;

namespace UniLaunch.Core.Storage;

public class BinaryStorageProvider<T> : IStorageProvider<T>
{
    public void Persist(string identifier, T data)
    {
        using var ms = File.OpenWrite(identifier + ".bin");
        BinaryFormatter formatter = new BinaryFormatter();
        //It serialize the employee object  
#pragma warning disable SYSLIB0011
        formatter.Serialize(ms, data!);
#pragma warning restore SYSLIB0011
    }

    public T Load(string identifier)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using var fs = File.Open(identifier + ".bin", FileMode.Open);
#pragma warning disable SYSLIB0011
        return (T)formatter.Deserialize(fs);
#pragma warning restore SYSLIB0011
    }
}