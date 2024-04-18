namespace UniLaunch.Core.ConnectivityCheck;

public class NetworkConnectivityChecker
{
    private readonly Uri _endpoint;
    private readonly TimeSpan _timeout;
    private readonly HttpClient _httpClient;

    private readonly TimeSpan _delayBetweenRetries = TimeSpan.FromMilliseconds(50);
    private readonly TimeSpan _requestTimeout = TimeSpan.FromMilliseconds(500);

    public NetworkConnectivityChecker(Uri endpoint, TimeSpan? timeout)
    {
        _endpoint = endpoint;
        _timeout = timeout ?? ConnectivityCheckConfiguration.DefaultTimeout;

        _httpClient = new HttpClient();
        _httpClient.Timeout = _requestTimeout;
    }

    public NetworkConnectivityChecker(ConnectivityCheckConfiguration c)
        : this(c.EndpointOrDefault(), c.TimeoutOrDefault())
    {
    }

    private async Task<bool> CallEndpoint()
    {
        var response = await _httpClient.GetAsync(_endpoint);
        var statusCode = (int)response.StatusCode;
        return statusCode is >= 200 and < 400;
    }

    public async Task Check()
    {
        Exception? lastError = null;
        var elapsedTime = TimeSpan.Zero;
        var startTime = DateTime.Now;
        var retryCount = 0;

        while (elapsedTime < _timeout)
        {
            try
            {
                var isReachable = await CallEndpoint();
                if (isReachable)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                lastError = e;
            }

            if (retryCount > 1)
            {
                await Task.Delay(_delayBetweenRetries);
            }

            elapsedTime = DateTime.Now - startTime;
            retryCount++;
        }

        var errorMessage = $"Connectivity check failed with {retryCount} retries in {_timeout}";

        if (lastError != null)
        {
            throw new NetworkConnectivityCheckFailedException(errorMessage, lastError);
        }

        throw new NetworkConnectivityCheckFailedException(errorMessage);
    }
}