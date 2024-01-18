namespace UniLaunch.Core.Autostart;

public interface IAutoStartRegistrationProvider
{
    /// <summary>
    /// Register a executable for startup on the current platform
    /// </summary>
    /// <param name="arguments">Arguments to pass to the registration</param>
    /// <exception cref="AutoStartRegistrationException">Failed to register the executable for system autostart</exception>
    public void Register(List<string> arguments);
    
    /// <summary>
    /// Register a executable for startup on the current platform
    /// </summary>
    /// <exception cref="AutoStartRegistrationException">Failed to register the executable for system autostart</exception>
    public void Register();

    /// <summary>
    /// Remove registration for a executable
    /// </summary>
    /// <param name="arguments">Arguments passed to the registration</param>
    public void DeRegister(List<string> arguments);

    /// <summary>
    /// Remove registration for a executable
    /// </summary>
    public void DeRegister();
}