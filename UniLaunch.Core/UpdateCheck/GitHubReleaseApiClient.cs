using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace UniLaunch.Core.UpdateCheck;

public class GitHubReleaseApiClient
{
    private readonly Uri _latestReleasesEndpoint =
        new("https://api.github.com/repos/timo-reymann/UniLaunch/releases/latest");

    private readonly HttpClient _httpClient;

    public GitHubReleaseApiClient()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "timo-reymann/UniLaunch");
        _httpClient.Timeout = TimeSpan.FromSeconds(2);
    }

    public async Task<GitHubRelease?> GetLatestRelease()
    {
        var response = await _httpClient.GetAsync(_latestReleasesEndpoint);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new HttpRequestException(await response.Content.ReadAsStringAsync(),null, response.StatusCode);
        }
        
        return JsonConvert.DeserializeObject<GitHubRelease>(await response.Content.ReadAsStringAsync(),new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        });
    }
}