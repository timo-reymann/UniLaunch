namespace UniLaunch.Core.Util;

public static class PathUtil
{
   public static string UserHome => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
}