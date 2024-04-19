namespace UniLaunch.Core.ExclusiveInstance;

public class ExclusiveInstanceProvider : IDisposable
{
    private Mutex? _mutex;

    public ExclusiveInstanceProvider()
    {
        _mutex = new Mutex(false, "Global\\UniLaunchInstance");
    }

    public void Acquire()
    {
        if (!_mutex!.WaitOne(TimeSpan.FromSeconds(1), false))
        {
            throw new ExclusiveInstanceAcquireFailedException();
        }
    }

    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing || _mutex == null)
        {
            return;
        }

        _mutex.Dispose();
        _mutex = null;
    }
}