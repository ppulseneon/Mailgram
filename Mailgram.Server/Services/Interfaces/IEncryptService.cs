using System.Security.Cryptography;

namespace Mailgram.Server.Services.Interfaces;

public interface IEncryptService
{
    byte[] Encrypt(byte[] data, byte[] key, byte[] iv);
    byte[] Decrypt(byte[] data, byte[] key, byte[] iv);
    byte[] SignFile(byte[] fileData, RSAParameters privateKey);
    bool VerifyFileSign(byte[] fileData, byte[] signature, string publicKeyXml);
}