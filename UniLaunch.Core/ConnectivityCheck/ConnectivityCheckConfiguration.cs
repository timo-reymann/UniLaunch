namespace UniLaunch.Core.ConnectivityCheck;

public class ConnectivityCheckConfiguration
{
    public Uri? Endpoint { get; set; } = DefaultEndpoint;
    public TimeSpan? Timeout { get; set; } = DefaultTimeout;

    public static readonly Uri DefaultEndpoint = new("https://connectivity-check.timo-reymann.de/");
    public Uri EndpointOrDefault() => Endpoint ?? DefaultEndpoint;

    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(3);
    public TimeSpan TimeoutOrDefault() => Timeout ?? DefaultTimeout;
}