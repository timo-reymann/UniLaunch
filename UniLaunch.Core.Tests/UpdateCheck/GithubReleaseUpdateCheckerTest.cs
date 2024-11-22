using UniLaunch.Core.UpdateCheck;

namespace UniLaunch.Core.Tests.UpdateCheck;

public class GitHubReleaseApiClientTests
{
    [Fact]
    public async Task TestGetLatestRelease()
    {
        var apiClient = new GitHubReleaseApiClient();
        var release = await apiClient.GetLatestRelease();
        Assert.Matches("https://github.com/timo-reymann/UniLaunch/.*", release!.HtmlUrl);
        Assert.Matches(".*\\..*\\..*", release.TagName);
    }
}