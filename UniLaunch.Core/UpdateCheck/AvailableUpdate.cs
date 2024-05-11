namespace UniLaunch.Core.UpdateCheck;

public class AvailableUpdate
{
    public Version Version { get; set; }
    public Uri DownloadPage { get; set; }

    public static AvailableUpdate FromGitHubRelease(GitHubRelease gitHubRelease)
    {
        return new AvailableUpdate()
        {
            Version = new(gitHubRelease.TagName),
            DownloadPage = new(gitHubRelease.HtmlUrl),
        };
    } 
}