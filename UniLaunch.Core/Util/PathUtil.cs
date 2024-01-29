namespace UniLaunch.Core.Util;

/// <summary>
/// Utility around paths
/// </summary>
public static class PathUtil
{
   public static string UserHome => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
}