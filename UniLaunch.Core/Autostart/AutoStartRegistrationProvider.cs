namespace UniLaunch.Core.Autostart;

public abstract class AutoStartRegistrationProvider
{
    /// <summary>
    /// Register a executable for startup on the current platform
    /// </summary>
    /// <param name="arguments">Arguments to pass to the registration</param>
    /// <exception cref="AutoStartRegistrationException">Failed to register the executable for system autostart</exception>
    public abstract void Register(List<string> arguments);

    /// <summary>
    /// Register a executable for startup on the current platform
    /// </summary>
    /// <exception cref="AutoStartRegistrationException">Failed to register the executable for system autostart</exception>
    public void Register()
    {
        Register(new List<string>());
    }

    /// <summary>
    /// Remove registration for a executable
    /// </summary>
    /// <param name="arguments">Arguments passed to the registration</param>
    public abstract void DeRegister(List<string> arguments);

    /// <summary>
    /// Remove registration for a executable
    /// </summary>
    public void DeRegister()
    {
        DeRegister(new List<string>());
    }
}