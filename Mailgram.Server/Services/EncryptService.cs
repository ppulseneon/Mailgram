using System.Security.Cryptography;
using Mailgram.Server.Services.Interfaces;
using Mailgram.Server.Tools;

namespace Mailgram.Server.Services;

public class EncryptService: IEncryptService
{
    public async Task EncryptMessage()
    {
        throw new NotImplementedException();
    }

    public async Task EncryptFile(string filepath)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GenerateRsa(string filepath, string privateKeyName)
    {
        // Получаем директорию для сохранения результатов шифрования
        var folder = FilePathHelper.GetPathFromFilepath(filepath);
        
        // Генерация приватного и публичного ключа при помощи RSA
        using RSACryptoServiceProvider rsaCryptoServiceProvider = new();

        // Экспорт публичного RSA ключа
        var publicKeyXml = rsaCryptoServiceProvider.ToXmlString(false);

        // Экспорт приватного RSA ключа
        var privateKeyXml = rsaCryptoServiceProvider.ToXmlString(true);
        
        var privatePath = Path.Combine(filepath, privateKeyName);
        await File.WriteAllTextAsync(privatePath, privateKeyXml);
        
        return publicKeyXml;
    }
    
    private async Task GenerateDes(string filepath)
    {
        // Получаем директорию для сохранения результатов шифрования
        var folder = FilePathHelper.GetPathFromFilepath(filepath);
        
        // Получаем название файла для подставки расширений результатов шифрования
        var filename = FilePathHelper.GetFileName(filepath);
        
        // Генерируем DES ключ
        using var des = TripleDES.Create();
        
        // Получаем путь к файлу DES ключа
        var keyPath =  Path.Combine(folder, $"{filename}.des");
        
        // Сохраняем содержимое ключа
        await File.WriteAllBytesAsync(keyPath, des.Key);
        
        // Получаем путь к вектору инициализации
        var ivPath =  Path.Combine(folder, $"{filename}.iv");
        
        // Сохраняем вектор инициализации
        await File.WriteAllBytesAsync(keyPath, des.IV);
    }
    
    public string DecryptFile(string filepath)
    {
        throw new NotImplementedException();
    }

    public string CreateMessageSign()
    {
        throw new NotImplementedException();
    }

    public bool VerifySign()
    {
        throw new NotImplementedException();
    }
}