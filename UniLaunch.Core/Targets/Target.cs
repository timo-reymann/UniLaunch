namespace UniLaunch.Core.Targets;

[Serializable]
public abstract class Target
{
    public string Name { get; set; }

    /// <summary>
    /// Start up the given target. The implementation must start the target detached.
    /// </summary>
    /// <exception cref="TargetInvocationFailedException">Target could not be invoked</exception>
    public abstract Task<TargetInvokeResult> Invoke();

    protected TargetInvokeResult Success() => new TargetInvokeResult(this, TargetInvokeResultStatus.Success);

    protected TargetInvokeResult Error(Error[] errors) =>
        new TargetInvokeResult(this, TargetInvokeResultStatus.Failure, Errors: errors);

    protected TargetInvokeResult Error(Exception e) => Error(new Error[]
    {
        new(e.GetType().FullName ?? "Unknown", e.Message)
    });
}