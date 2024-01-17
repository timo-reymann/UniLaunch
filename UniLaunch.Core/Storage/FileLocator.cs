namespace UniLaunch.Core.Storage;

public class FileLocator
{
    private List<String> LocationsToSearch { get; set; }
    private string FallbackLocation { get; set; }

    public FileLocator(List<string> locationsToSearch, string fallbackLocation)
    {
        LocationsToSearch = locationsToSearch;
        FallbackLocation = fallbackLocation;
    }

    public FileLocator(List<string> locationsToSearch)
        : this(locationsToSearch, locationsToSearch.First())
    {
    }

    private bool LocationExists(string location, string extension) => File.Exists($"{location}.{extension}");

    public string? Locate(string extension) => LocationsToSearch.FirstOrDefault(ls => LocationExists(ls, extension));

    public string LocateWithFallback(string extension) => Locate(extension) ?? FallbackLocation;
}