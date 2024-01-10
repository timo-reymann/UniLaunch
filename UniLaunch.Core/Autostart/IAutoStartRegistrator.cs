namespace UniLaunch.Core.Autostart;

public interface IAutoStartRegistrator
{
    /// <summary>
    /// Register a executable for startup on the current platform
    /// </summary>
    /// <param name="executable">Full qualified path of the executable</param>
    /// <param name="arguments">Arguments to pass to the registration</param>
    /// <exception cref="AutoStartRegistrationFailed">Failed to register the executable for system autostart</exception>
    public void Register(string executable, List<string> arguments);


    /// <summary>
    /// Register a executable for startup on the current platform
    /// </summary>
    /// <param name="executable">Full qualified path of the executable</param>
    /// <exception cref="AutoStartRegistrationFailed">Failed to register the executable for system autostart</exception>
    public void Register(string executable);
}