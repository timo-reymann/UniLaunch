namespace UniLaunch.Linux.Desktop;

public class DesktopFileWriter : IDisposable
{
    private readonly StreamWriter _writer;

    public DesktopFileWriter(string path)
    {
        _writer = new StreamWriter(path);
        _writer.WriteLine("[Desktop Entry]");
    }

    public void Write(string key, string value)
    {
        _writer.WriteLine($"{key}={value}");
    }

    public void Flush() => _writer.Flush();
    
    public void Dispose()
    {
        _writer.Dispose();
    }
}