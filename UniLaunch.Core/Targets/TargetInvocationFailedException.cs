namespace UniLaunch.Core.Targets;

public class TargetInvocationFailedException : Exception
{
    public TargetInvocationFailedException(string message) : base(message)
    {
    }

    public TargetInvocationFailedException(string message, Exception cause) : base(message, cause)
    {
    }
}