using System.Security.Cryptography;
using Mailgram.Server.Services.Interfaces;

namespace Mailgram.Server.Services;

public class EncryptService: IEncryptService
{
    public byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
    {
        using var des = TripleDES.Create();
        des.Key = key;
        des.IV = iv;

        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
        cs.Write(data, 0, data.Length);
        cs.FlushFinalBlock();
        return ms.ToArray();
    }

    public byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
    {
        using var des = TripleDES.Create();
        des.Key = key;
        des.IV = iv;

        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
        cs.Write(data, 0, data.Length);
        cs.FlushFinalBlock();
        return ms.ToArray();
    }

    public byte[] SignFile(byte[] fileData, RSAParameters privateKey)
    {
        using var rsa = new RSACryptoServiceProvider();
        rsa.ImportParameters(privateKey);
        using var sha256 = SHA256.Create();
        return rsa.SignData(fileData, sha256);
    }

    public bool VerifyFileSign(byte[] fileData, byte[] signature, string publicKeyXml)
    {
        using var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(publicKeyXml);
        using var sha256 = SHA256.Create();
        return rsa.VerifyData(fileData, sha256, signature);
    }
}