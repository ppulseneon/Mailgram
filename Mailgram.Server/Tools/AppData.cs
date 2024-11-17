using System.Security.Cryptography;
using System.Text;
using Mailgram.Server.Constants;
using Mailgram.Server.Models;
using Mailgram.Server.Tools;
using Newtonsoft.Json;

namespace Mailgram.Server.Utility;

public static class AppData
{
    private const string KeysFolder = "keys";
    private const string Temp = "temp";
    private const string DesEncryptedFilename = "encrypt.key";  
    private const string PrivateKeyFilename = "private.key";
    private const string PublicKeyFilename = "public.key";
    private const string IvFilename = "IV.key";
    
    public static string GetAppDataDirectory()
    {
        const string appName = SystemFoldersNames.Application;
        
        var appDataDirectory = Environment.OSVersion.Platform switch
        {
            PlatformID.Win32NT => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                $".{appName}", appName),
            PlatformID.Unix => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config",
                appName, appName),
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
        Directory.CreateDirectory(Temp);
        
        using var des = TripleDES.Create();
        var desKey = des.Key;
        var iv = des.IV;

        // Сохраняем IV в файл
        var ivPath = Path.Combine(keysDirectory, IvFilename);
        File.WriteAllBytes(ivPath, iv);
        
        // Генерация приватного и публичного ключа при помощи RSA
        using RSACryptoServiceProvider rsaCryptoServiceProvider = new();

        // Экспорт публичного RSA ключа
        var publicKeyXml = rsaCryptoServiceProvider.ToXmlString(false);
        var publicPath = Path.Combine(keysDirectory, PublicKeyFilename);
        File.WriteAllText(publicPath, publicKeyXml);

        // Экспорт приватного RSA ключа
        var privateKeyXml = rsaCryptoServiceProvider.ToXmlString(true);
        var privatePath = Path.Combine(keysDirectory, PrivateKeyFilename);
        File.WriteAllText(privatePath, privateKeyXml);
        
        // Экспорт зашифрованного DES ключа
        var desEncryptKey = rsaCryptoServiceProvider.Encrypt(desKey, RSAEncryptionPadding.Pkcs1);

        // Экспортируем зашифрованный dsa key
        var rsa = Path.Combine(keysDirectory, DesEncryptedFilename);
        File.WriteAllBytes(rsa, desEncryptKey);
    }

    public static string CreateSubFolder()
    {
        var appDataDirectory = GetAppDataDirectory();
        var tempDirectory = Path.Combine(appDataDirectory, Temp);
        var path = Path.Combine(tempDirectory, Guid.NewGuid().ToString());
        Directory.CreateDirectory(path);

        return path;
    }
    
    public static async Task SaveEncryptedFile(string jsonContent, string encryptedFilePath)
    {
        var data = Encoding.UTF8.GetBytes(jsonContent);
        await SaveEncryptedFile(data, encryptedFilePath);
    }
    
    public static async Task SaveEncryptedFile(byte[] data, string encryptedFilePath)
    {
        var appDataDirectory = GetAppDataDirectory();
        var keysDirectory = Path.Combine(appDataDirectory, KeysFolder);
        
        var iv = await File.ReadAllBytesAsync(Path.Combine(keysDirectory, IvFilename));
        var desKey = await File.ReadAllBytesAsync(Path.Combine(keysDirectory, DesEncryptedFilename));
        var privateKeyXml = await File.ReadAllTextAsync(Path.Combine(keysDirectory, PrivateKeyFilename));
        
        // Создаем RSA провайдер
        using var rsa = new RSACryptoServiceProvider();
        
        // Экспортируем приватный ключ
        rsa.FromXmlString(privateKeyXml);
        
        // Дешифруем зашифрованный при инициализации ключ DES
        var decryptedDesKey = rsa.Decrypt(desKey, RSAEncryptionPadding.Pkcs1);
        
        // Шифруем расшифрованным DES ключом данные файла
        var encryptedData = TripleDesCrypt.Encrypt(data, decryptedDesKey, iv);

        // Записываем в файл зашифрованный файл
        await File.WriteAllBytesAsync(encryptedFilePath, encryptedData);
    }
    
    public static async Task<T?> ReadEncryptedFile<T>(string filePath)
    {
        var decryptedData = await ReadEncryptedFileData(filePath);
        
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
    
    public static async Task<byte[]> ReadEncryptedFileData(string filePath)
    {
        var appDataDirectory = GetAppDataDirectory();
        var keysDirectory = Path.Combine(appDataDirectory, KeysFolder);
        
        var privateKeyXml = await File.ReadAllTextAsync(Path.Combine(keysDirectory, PrivateKeyFilename));
        var iv  = await File.ReadAllBytesAsync(Path.Combine(keysDirectory, IvFilename));
        var desKey = await File.ReadAllBytesAsync(Path.Combine(keysDirectory, DesEncryptedFilename));

        // Создаем RSA провайдер
        using var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(privateKeyXml);
        
        // Дешифруем зашифрованный при инициализации ключ DES
        var decryptedDesKey = rsa.Decrypt(desKey, RSAEncryptionPadding.Pkcs1);
        
        // Получаем зашифрованные данные файла
        var encryptedData = await File.ReadAllBytesAsync(filePath);
        
        // Дешифруем зашифрованные файлы расшифрованным DES-ключом 
        var decryptedData = TripleDesCrypt.Decrypt(encryptedData, decryptedDesKey, iv);
        
        return decryptedData;
    }
}