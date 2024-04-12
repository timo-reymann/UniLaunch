namespace UniLaunch.Core.ConnectivityCheck;

public record ConnectivityCheckConfiguration
{
    public Uri? Endpoint;
    public TimeSpan? Timeout;

    public static readonly Uri DefaultEndpoint = new("https://detectportal.firefox.com");
    public Uri EndpointOrDefault() => Endpoint ?? DefaultEndpoint;

    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(3);
    public TimeSpan TimeoutOrDefault() => Timeout ?? DefaultTimeout;
}