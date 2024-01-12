using System.Diagnostics;
using Microsoft.VisualBasic;

namespace UniLaunch.Core.Targets;

[Serializable]
public class ApplicationTarget : ITarget
{
    public string? Executable { get; set; }
    public string[]? Arguments { get; set; }

    public string Name { get; set; }

    public void Invoke()
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                Arguments = Strings.Join(Arguments ?? Array.Empty<string>(), " "),
                UseShellExecute = true,
                FileName = Executable ?? throw new TargetInvocationFailedException("Executable not set")
            });
        }
        catch (Exception e)
        {
            throw new TargetInvocationFailedException("Could not start application", e);
        }
    }
}