using System.Security.Cryptography;
using System.Text;
using Mailgram.Server.Services.Interfaces;
using Mailgram.Server.Tools;

namespace Mailgram.Server.Services;

public class EncryptService: IEncryptService
{
    public async Task<string> EncryptMessage(string message, byte[] desKey, byte[] iv)
    {
        using var des = TripleDES.Create();
        des.Key = desKey;
        des.IV = iv;

        using var ms = new MemoryStream();
        await using (var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
        {
            await using var sw = new StreamWriter(cs, Encoding.UTF8); 
            await sw.WriteAsync(message); 
            await sw.FlushAsync(); 
            await cs.FlushFinalBlockAsync(); 
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public async Task<string> EncryptFile(IFormFile file, string subTempFolderName, byte[] desKey, byte[] iv)
    {
        using var des = TripleDES.Create();
        des.Key = desKey;
        des.IV = iv;

        using var ms = new MemoryStream();
        await using var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
        await file.CopyToAsync(cs);
        await cs.FlushFinalBlockAsync();

        var resultPath = Path.Combine(subTempFolderName, $"{file.FileName}.enc");
        await File.WriteAllBytesAsync(resultPath, ms.ToArray());
        
        return resultPath;
    }

    public async Task<string> EncryptFile(byte[] data, string subTempFolderName, string filename, byte[] desKey, byte[] iv)
    {
        using var des = TripleDES.Create();
        des.Key = desKey;
        des.IV = iv;

        using var ms = new MemoryStream();
        await using (var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
        {
            await cs.WriteAsync(data);
            await cs.FlushFinalBlockAsync();
        }

        var resultPath = Path.Combine(subTempFolderName, filename);
        await File.WriteAllBytesAsync(resultPath, ms.ToArray());
        
        return resultPath;
    }
    
    public async Task<string> EncryptKey(byte[] desKey, string subTempFolderName, string publicRsaKeyPath)
    {
        using var rsa = new RSACryptoServiceProvider();

        var publicRsa = await File.ReadAllTextAsync(publicRsaKeyPath);
        
        // Экспортируем публичный ключ
        rsa.FromXmlString(publicRsa);

        var encryptedDesa = rsa.Encrypt(desKey, RSAEncryptionPadding.Pkcs1);
        
        var resultPath = Path.Combine(subTempFolderName, ".key");
        await File.WriteAllBytesAsync(resultPath, encryptedDesa.ToArray());
        
        return resultPath;
    }

    public async Task<string> SaveIv(byte[] iv, string subTempFolderName)
    {
        var resultPath = Path.Combine(subTempFolderName, ".iv");
        await File.WriteAllBytesAsync(resultPath, iv.ToArray());
        
        return resultPath;
    }

    public async Task<string> SaveSign(string data, string subTempFolderName)
    {
        var resultPath = Path.Combine(subTempFolderName, ".sign");
        await File.WriteAllTextAsync(resultPath, data);
        
        return resultPath;
    }

    public async Task<byte[]> DecryptKey(byte[] desKey, string privateRsaKeyPath)
    {
        using var rsa = new RSACryptoServiceProvider();

        var unformattedEmail = privateRsaKeyPath.Split("\\")[^2];;
        var formattedEmail = unformattedEmail.Substring(unformattedEmail.IndexOf('<') + 1, unformattedEmail.IndexOf('>') - unformattedEmail.IndexOf('<') - 1);
        
        var publicRsa = await File.ReadAllTextAsync(privateRsaKeyPath.Replace(unformattedEmail, formattedEmail));
        
        // Экспортируем приватный ключ
        rsa.FromXmlString(publicRsa);
        
        var decryptedDes = rsa.Decrypt(desKey, RSAEncryptionPadding.Pkcs1);
        return decryptedDes;
    }

    public async Task<byte[]> DecryptFile(byte[] data, byte[] desKey, byte[] iv)
    {
        using var des = TripleDES.Create();
        des.Key = desKey;
        des.IV = iv;

        using var ms = new MemoryStream(data);
        await using var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);

        using var decryptedMemoryStream = new MemoryStream();
        await cs.CopyToAsync(decryptedMemoryStream);

        return decryptedMemoryStream.ToArray();
    }

    public byte[] ComputeSHA256Hash(string data)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(data);
        return sha256.ComputeHash(bytes);
    }

    public async Task<string> DecryptMessage(string message, byte[] desKey, byte[] iv)
    {
        using var des = TripleDES.Create();
        des.Key = desKey;
        des.IV = iv;

        using var ms = new MemoryStream(Convert.FromBase64String(message));
        await using var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
        using var sr = new StreamReader(cs, Encoding.UTF8);
        return await sr.ReadToEndAsync();
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

    public async Task<(byte[], byte[])> GenerateTripleDes()
    {
        using var des = TripleDES.Create();
        
        var keyResult = des.Key;
        var ivResult = des.IV;
        
        return (keyResult, ivResult);
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

    public async Task<string> CreateMessageSign(string hash, string privateEcpKeyPath)
    {
        using var rsa = RSA.Create();
        var privateRsaEcp = await File.ReadAllTextAsync(privateEcpKeyPath);
        rsa.FromXmlString(privateRsaEcp);
        
        var hashBytes = Convert.FromHexString(hash);
        var signatureBytes = rsa.SignHash(hashBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        
        return Convert.ToBase64String(signatureBytes);
    }

    public async Task<bool> VerifySign(byte[] hash, byte[] signature, string publicKeyXml)
    {
        using var rsa = RSA.Create();
        
        var unformattedEmail = publicKeyXml.Split("\\")[^2];
        var formattedEmail = unformattedEmail.Substring(unformattedEmail.IndexOf('<') + 1, unformattedEmail.IndexOf('>') - unformattedEmail.IndexOf('<') - 1);
        var publicRsaEcp = await File.ReadAllTextAsync(publicKeyXml.Replace(unformattedEmail, formattedEmail));
        
        rsa.FromXmlString(publicRsaEcp);
        return rsa.VerifyHash(hash, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }
}