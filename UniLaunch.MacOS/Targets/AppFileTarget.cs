using System.Diagnostics;
using UniLaunch.Core.Storage.Serialization;
using UniLaunch.Core.Targets;

namespace UniLaunch.MacOS.Targets;

[PropertyValueForSerialization("appFile")]
public class AppFileTarget : Target
{
    public override string TargetType => "appFile";

    public string Path { get; set; } = null!;

    public override async Task<TargetInvokeResult> Invoke()
    {
        try
        {
            var process = Process.Start(new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardError = true,
                FileName = "/usr/bin/open",
                Arguments = $"{Path}"
            });
            process!.Start();

            await process.WaitForExitAsync(new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token);
            if (process.ExitCode != 0)
            {
                return Error(new Error[]
                {
                    new("OpenFailed", process.StandardError.ReadToEnd())
                });
            }
        }
        catch (Exception e)
        {
            return Error(e);
        }

        return Success();
    }

}