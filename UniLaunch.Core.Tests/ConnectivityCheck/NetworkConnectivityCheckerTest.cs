using UniLaunch.Core.ConnectivityCheck;

namespace UniLaunch.Core.Tests.ConnectivityCheck;

public class NetworkConnectivityCheckerTest
{
    [Fact]
    public async Task CheckUnreachable()
    {
        var checker = new NetworkConnectivityChecker(
            new Uri("https://no-host.timo-reymann.de"),
            TimeSpan.FromMilliseconds(10)
        );
        var exc = await Assert.ThrowsAsync<NetworkConnectivityCheckFailedException>(async () => await checker.Check());
        Assert.Matches("Connectivity check failed with \\d retries", exc.Message);
    }

    [Fact]
    public async Task CheckReachable()
    {
        var checker = new NetworkConnectivityChecker(
            new Uri("https://one.one.one.one/help/"),
            TimeSpan.FromMilliseconds(10)
        );
        await checker.Check();
        Assert.True(true);
    }
}