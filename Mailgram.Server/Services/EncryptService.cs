using System.Security.Cryptography;
using Mailgram.Server.Services.Interfaces;
using Mailgram.Server.Tools;

namespace Mailgram.Server.Services;

public class EncryptService: IEncryptService
{
    public Task EncryptMessage()
    {
        throw new NotImplementedException();
    }

    public async Task EncryptFile(string filepath)
    {

    }

    private async Task GenerateDES(string filepath)
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