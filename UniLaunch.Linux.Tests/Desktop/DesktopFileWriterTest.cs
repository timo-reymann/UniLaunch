using UniLaunch.Linux.Desktop;

namespace UniLaunch.Linux.Tests.Desktop;

public class DesktopFileWriterTest : IDisposable
{
    private readonly string _tempFilePath;
    private readonly DesktopFileWriter _desktopFileWriter;

    public DesktopFileWriterTest()
    {
        _tempFilePath = Path.GetTempFileName(); 
        _desktopFileWriter = new DesktopFileWriter(_tempFilePath);
    }

    [Fact]
    public void Write_AddsKeyAndValueToDesktopFile()
    {
        const string key = "TestKey";
        const string value = "TestValue";

        _desktopFileWriter.Write(key, value);
        _desktopFileWriter.Flush();
        var fileContent = File.ReadAllText(_tempFilePath);
        Assert.Contains($"{key}={value}", fileContent);
    }

    public void Dispose()
    {
        _desktopFileWriter.Dispose();
        File.Delete(_tempFilePath);
        GC.SuppressFinalize(this);
    }
}