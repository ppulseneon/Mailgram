namespace Mailgram.Server.Tools;

public static class MailDomains
{
    public static string AppendDomain(this string email, string platform)
    {
        return platform switch
        {
            "Yandex" => $"{email}@yandex.ru",
            "Rambler" => $"{email}@rambler.ru",
            _ => throw new Exception("Platform not supported")
        };
    }
}