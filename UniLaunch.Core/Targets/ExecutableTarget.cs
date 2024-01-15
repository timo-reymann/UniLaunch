using System.Diagnostics;
using Microsoft.VisualBasic;

namespace UniLaunch.Core.Targets;

[Serializable]
public class ExecutableTarget : Target
{
    public string? Executable { get; set; }
    public string[]? Arguments { get; set; }

    public override string ConfigName => "executable";

    public override Task<TargetInvokeResult> Invoke()
    {
        try
        {
            var process = Process.Start(new ProcessStartInfo
            {
                Arguments = Strings.Join(Arguments ?? Array.Empty<string>(), " "),
                UseShellExecute = false,
                RedirectStandardError = false,
                RedirectStandardInput = false,
                RedirectStandardOutput = false,
                FileName = Executable ?? throw new TargetInvocationFailedException("Executable not set")
            });
            if (process == null)
            {
                return Task.FromResult(Error(new Error[]
                {
                    new("ProcessSpawnFailed")
                }));
            }
        }
        catch (Exception e)
        {
            return Task.FromResult(Error(e));
        }

        return Task.FromResult(Success());
    }
}