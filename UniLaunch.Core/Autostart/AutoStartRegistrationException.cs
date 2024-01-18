namespace UniLaunch.Core.Autostart;

public class AutoStartRegistrationException : Exception
{
    public AutoStartRegistrationException() {}

    public AutoStartRegistrationException(string message) : base(message)
    {
        
    }

    public AutoStartRegistrationException(string message, Exception cause) : base(message, cause)
    {
        
    }
}