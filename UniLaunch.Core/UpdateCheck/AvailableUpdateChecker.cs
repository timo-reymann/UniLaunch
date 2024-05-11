using UniLaunch.Core.Meta;

namespace UniLaunch.Core.UpdateCheck;

public class AvailableUpdateChecker
{
    private GitHubReleaseApiClient _gitHubReleaseApiClient = new();
    private AppInfoProvider _appInfoProvider = new();

    public async Task<Tuple<bool, AvailableUpdate?>> CheckAvailableUpdate()
    {
        GitHubRelease? latestRelease = null;

        try
        {
            latestRelease = await _gitHubReleaseApiClient.GetLatestRelease();
        }
        catch
        {
            // ignore
        }

        if (latestRelease == null)
        {
            return await Task.FromResult(new Tuple<bool, AvailableUpdate?>(false, null));
        }

        var latestAvailableRelease = AvailableUpdate.FromGitHubRelease(latestRelease);
        return new Tuple<bool, AvailableUpdate?>(
            IsNewerThanCurrent(latestAvailableRelease.Version),
            latestAvailableRelease
        );
    }

    private bool IsNewerThanCurrent(Version other)
    {
        var currentVersion = new Version(_appInfoProvider.VersionInfo.ProductVersion!.Split("+")[0]);
        return other.CompareTo(currentVersion) > 0;
    }
}