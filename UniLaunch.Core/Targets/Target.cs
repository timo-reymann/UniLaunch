using Newtonsoft.Json;
using UniLaunch.Core.Storage;
using UniLaunch.Core.Storage.JSON;

namespace UniLaunch.Core.Targets;

[JsonConverter(typeof(TargetConverter))]
[Serializable]
public abstract class Target
{
    public string Name { get; set; }
    
    public abstract string ConfigName { get; }

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