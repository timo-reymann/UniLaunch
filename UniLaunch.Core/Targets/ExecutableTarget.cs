using System.Diagnostics;
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

    public override async Task<TargetInvokeResult> Invoke()
    {
        Process? process = null;
        try
        {
            var executable = Executable ?? throw new TargetInvocationFailedException("Executable not set");
            process = Process.Start(CreateStartInfo(executable, Arguments ?? Array.Empty<string>()));
            if (process == null)
            {
                return Error(new Error[] { new("ProcessSpawnFailed") });
            }

            if (!OperatingSystem.IsWindows())
            {
                // Wait for the shell wrapper to confirm it found the executable. The
                // wrapper redirects the target's streams to /dev/null before exec, so
                // the target never depends on pipes owned by this short-lived launcher.
                var started = await process.StandardOutput.ReadLineAsync();
                if (started is null)
                {
                    await process.WaitForExitAsync();
                    return Error(new Error[]
                    {
                        new("ProcessSpawnFailed", $"Could not start executable '{executable}'")
                    });
                }

                process.StandardOutput.Close();
            }

            return Success();
        }
        catch (Exception e)
        {
            return Error(e);
        }
        finally
        {
            process?.Dispose();
        }
    }

    private static ProcessStartInfo CreateStartInfo(string executable, string[] arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            UseShellExecute = false
        };

        if (OperatingSystem.IsWindows())
        {
            startInfo.FileName = executable;
            foreach (var argument in arguments)
            {
                startInfo.ArgumentList.Add(argument);
            }

            return startInfo;
        }

        string? setsid = null;
        if (OperatingSystem.IsLinux())
        {
            if (File.Exists("/usr/bin/setsid"))
            {
                setsid = "/usr/bin/setsid";
            }
            else if (File.Exists("/bin/setsid"))
            {
                setsid = "/bin/setsid";
            }
        }

        startInfo.FileName = setsid ?? "/bin/sh";
        if (setsid is not null)
        {
            startInfo.ArgumentList.Add("/bin/sh");
        }

        startInfo.RedirectStandardOutput = true;
        startInfo.ArgumentList.Add("-c");
        startInfo.ArgumentList.Add(
            "if [ -x \"$1\" ]; then :; else command -v \"$1\" >/dev/null 2>&1 || exit 127; fi; " +
            "printf 'unilaunch-started\\n'; exec \"$@\" </dev/null >/dev/null 2>&1");
        startInfo.ArgumentList.Add("unilaunch");
        startInfo.ArgumentList.Add(executable);
        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        return startInfo;
    }
}
