namespace Mailgram.Server.Tools;

public static class FilePathHelper
{
    public static string? GetPathFromFilepath(string filePath)
    {
        return string.IsNullOrEmpty(filePath) ? string.Empty : Path.GetDirectoryName(filePath);
    }
    
    public static string GetFileName(string filePath)
    {
        return string.IsNullOrEmpty(filePath) ? string.Empty :
            Path.GetFileNameWithoutExtension(filePath);
    }
}