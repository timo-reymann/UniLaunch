namespace UniLaunch.Linux.Desktop;

public class DesktopFileWriter : IDisposable
{
    private StreamWriter writer;

    public DesktopFileWriter(string path)
    {
        writer = new StreamWriter(path);
        writer.WriteLine("[Desktop Entry]");
    }

    public void Write(string key, string value)
    {
        writer.WriteLine($"{key}={value}");
    }

    public void Flush() => writer.Flush();
    
    public void Dispose()
    {
        writer.Dispose();
    }
}