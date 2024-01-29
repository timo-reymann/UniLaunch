using UniLaunch.Linux.Desktop;

namespace UniLaunch.Linux.Tests.Desktop;


public class XdgConfigTest
{
    [Fact]
    public void UserConfigFolder_UsesPathUtilUserHomeWhenEnvironmentVariableNotSet()
    {
        Environment.SetEnvironmentVariable("XDG_CONFIG_HOME", null); // Ensure XDG_CONFIG_HOME is not set
        Environment.SetEnvironmentVariable("HOME", "foo");
        
        Assert.EndsWith("/.config",  XdgConfig.UserConfigFolder);
    }

    [Fact]
    public void UserConfigFolder_UsesXDGConfigHomeWhenEnvironmentVariableSet()
    {
        const string customXdgConfigHome = "/custom/config/home";
        
        Environment.SetEnvironmentVariable("XDG_CONFIG_HOME", customXdgConfigHome);
        Assert.Equal(customXdgConfigHome,  XdgConfig.UserConfigFolder);
    }

    [Fact]
    public void UserConfigFolder_UsesXDGPathWhenBothVariablesAreSet()
    {
        const string customXdgConfigHome = "/custom/config/home";
        Environment.SetEnvironmentVariable("XDG_CONFIG_HOME", customXdgConfigHome);
        Environment.SetEnvironmentVariable("HOME", "foo");

        Assert.Equal(customXdgConfigHome, XdgConfig.UserConfigFolder);
    }
}