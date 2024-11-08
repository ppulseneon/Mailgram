using System.Security.Cryptography;
using System.Text;
using Mailgram.Server.Tools;
using Newtonsoft.Json;

namespace Mailgram.Server.Utility;

public static class AppData
{
    private const string KeysFolder = "keys";
    private const string DesEncryptedFilename = "encrypt.key";  
    private const string PrivateKeyFilename = "private.key";
    private const string PublicKeyFilename = "public.key";
    private const string IVFilename = "IV.key";
    
    public static string GetAppDataDirectory()
    {
        var appDataDirectory = Environment.OSVersion.Platform switch
        {
            PlatformID.Win32NT => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".mailgram", "mailgram"),
            PlatformID.Unix => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config",
                "mailgram", "mailgram"),
            _ => string.Empty
        };

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

        using var des = TripleDES.Create();
        var desKey = des.Key;
        var iv = des.IV;

        // Сохраняем DES-ключ и IV в файлы
        var ivPath = Path.Combine(keysDirectory, IVFilename);
        File.WriteAllBytes(ivPath, iv);
        
        // Генерация приватного и публичного ключа при помощи RSA
        using RSACryptoServiceProvider rsaCryptoServiceProvider = new();
        var rsaParameters = rsaCryptoServiceProvider.ExportParameters(true);

        // Экспорт публичного
        var publicKeyXml = rsaCryptoServiceProvider.ToXmlString(false);
        var publicPath = Path.Combine(keysDirectory, PublicKeyFilename);
        File.WriteAllText(publicPath, publicKeyXml);

        // Экспорт приватного 
        var privateKeyXml = rsaCryptoServiceProvider.ToXmlString(true);
        var privatePath = Path.Combine(keysDirectory, PrivateKeyFilename);
        File.WriteAllText(privatePath, privateKeyXml);
        
        var desEncryptKey = rsaCryptoServiceProvider.Encrypt(desKey, RSAEncryptionPadding.Pkcs1);

        // Экспортируем зашифрованный dsa key
        var rsa = Path.Combine(keysDirectory, DesEncryptedFilename);
        File.WriteAllBytes(rsa, desEncryptKey);
    }

    public static async Task SaveEncryptedSystemFile(string jsonContent, string encryptedFilePath)
    {
        var data = Encoding.UTF8.GetBytes(jsonContent);
        
        var appDataDirectory = GetAppDataDirectory();
        var keysDirectory = Path.Combine(appDataDirectory, KeysFolder);
        
        var iv = await File.ReadAllBytesAsync(Path.Combine(keysDirectory, IVFilename));
        var desKey = await File.ReadAllBytesAsync(Path.Combine(keysDirectory, DesEncryptedFilename));
        var privateKeyXml = await File.ReadAllTextAsync(Path.Combine(keysDirectory, PrivateKeyFilename));
        
        using var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(privateKeyXml);
        var decryptedDesKey = rsa.Decrypt(desKey, RSAEncryptionPadding.Pkcs1);
        
        var encryptedData = Encrypter.Encrypt(data, decryptedDesKey, iv);

        await File.WriteAllBytesAsync(encryptedFilePath, encryptedData);
    }

    // todo: save encrypted user simple attachment 
    
    public static async Task<T?> ReadEncryptedSystemfile<T>(string filePath)
    {
        var appDataDirectory = GetAppDataDirectory();
        var keysDirectory = Path.Combine(appDataDirectory, KeysFolder);
        
        var privateKeyXml = await File.ReadAllTextAsync(Path.Combine(keysDirectory, PrivateKeyFilename));
        var iv  = await File.ReadAllBytesAsync(Path.Combine(keysDirectory, IVFilename));
        var desKey = await File.ReadAllBytesAsync(Path.Combine(keysDirectory, DesEncryptedFilename));

        using var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(privateKeyXml);
        
        var decryptedDesKey = rsa.Decrypt(desKey, RSAEncryptionPadding.Pkcs1);
        
        var encryptedData = await File.ReadAllBytesAsync(filePath);
        var decryptedData = Encrypter.Decrypt(encryptedData, decryptedDesKey, iv);
        
        var decryptedJson = Encoding.UTF8.GetString(decryptedData);
        try
        {
            return JsonConvert.DeserializeObject<T>(decryptedJson);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error deserializing JSON: {ex.Message}");
            throw;
        }
    }
}