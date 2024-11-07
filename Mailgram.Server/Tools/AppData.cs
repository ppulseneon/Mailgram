namespace Mailgram.Server.Utility;

public static class AppData
{
    private const string KeysFolder = "keys";
    
    public static string GetAppDataDirectory()
    {
        string appDataDirectory = string.Empty;
        
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".mailgram", "mailgram");
        }
        else if (Environment.OSVersion.Platform == PlatformID.Unix)
        {
            appDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "mailgram", "mailgram");
        }
        
        if (!Directory.Exists(appDataDirectory))
        {
            Directory.CreateDirectory(appDataDirectory);
        }

        return appDataDirectory;
    }

    public static void InitAppKeys()
    {
        var appDataDirectory = GetAppDataDirectory();
        var keysDirectory = Path.Combine(appDataDirectory, KeysFolder);
        
        if (Directory.Exists(keysDirectory))
        {
            return;
        }
        
        Directory.CreateDirectory(keysDirectory);
        
    }
}