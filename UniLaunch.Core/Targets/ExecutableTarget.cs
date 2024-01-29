using System.Diagnostics;
using Microsoft.VisualBasic;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Storage.Serialization;

namespace UniLaunch.Core.Targets;

/// <summary>
/// Run a standalone binary
/// </summary>
[PropertyValueForSerialization("executable")]
public class ExecutableTarget : Target
{
    public string? Executable { get; set; }
    public string[]? Arguments { get; set; } = null;

    public override string TargetType => "executable";

    public override Task<TargetInvokeResult> Invoke()
    {
        try
        {
            var process = Process.Start(new ProcessStartInfo
            {
                Arguments = Strings.Join(Arguments ?? Array.Empty<string>(), " "),
                UseShellExecute = false,
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