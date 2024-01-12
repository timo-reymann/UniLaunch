namespace UniLaunch.Core.Targets;

public interface ITarget
{
    public string Name { get; set; }

    /// <summary>
    /// Start up the given target. The implementation must start the target detached.
    /// </summary>
    /// <exception cref="TargetInvocationFailedException">Target could not be invoked</exception>
    public void Invoke();
}