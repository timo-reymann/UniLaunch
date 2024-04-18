namespace UniLaunch.Core.ConnectivityCheck;

public class NetworkConnectivityCheckFailedException : Exception
{
    public NetworkConnectivityCheckFailedException(string message) 
        : base(message) { }

    public NetworkConnectivityCheckFailedException(string message, Exception e)
        : base(message, e) { }
}