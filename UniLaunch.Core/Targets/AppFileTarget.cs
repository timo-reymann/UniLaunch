using System.Diagnostics;
using Microsoft.VisualBasic;

namespace UniLaunch.Core.Targets;

public class AppFileTarget : Target
{
    public override string ConfigName => "appFile";
    
    public string Path { get; set; }

    public override Task<TargetInvokeResult> Invoke()
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
            process.Start();

            process.WaitForExit(3_000);
            if (process.ExitCode != 0)
            {
                return Task.FromResult(Error(new Error[]
                {
                    new("OpenFailed", process.StandardError.ReadToEnd())
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