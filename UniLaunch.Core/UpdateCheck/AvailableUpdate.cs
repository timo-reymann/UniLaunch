namespace UniLaunch.Core.UpdateCheck;

public class AvailableUpdate
{
    public Version Version { get; set; } = null!;
    public Uri DownloadPage { get; set; } = null!;

    public static AvailableUpdate FromGitHubRelease(GitHubRelease gitHubRelease)
    {
        return new AvailableUpdate()
        {
            Version = new(gitHubRelease.TagName!),
            DownloadPage = new(gitHubRelease.HtmlUrl!),
        };
    } 
}