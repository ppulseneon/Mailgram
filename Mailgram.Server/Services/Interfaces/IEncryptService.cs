namespace Mailgram.Server.Services.Interfaces;

public interface IEncryptService
{
    Task<string> GenerateRsa(string filepath, string privateKeyName);
    Task<(byte[], byte[])> GenerateTripleDes();
    Task<string> EncryptMessage(string message, byte[] desKey, byte[] iv);
    Task<string> EncryptFile(IFormFile file, string subTempFolderName, byte[] desKey, byte[] iv);
    Task<string> DecryptMessage(string message, byte[] desKey, byte[] iv);
    Task<string> EncryptKey(byte[] desKey, string subTempFolderName, string publicRsaKeyPath);
    Task<string> SaveIv(byte[] iv, string subTempFolderName);
    Task<string> SaveSign(byte[] data, string subTempFolderName);
    Task<byte[]> DecryptKey(byte[] desKey, string privateRsaKeyPath);
    Task<byte[]> DecryptFile(byte[] data, byte[] desKey, byte[] iv);
    byte[] ComputeSha256Hash(string data);
    Task<string> CreateMessageSign(string hash, string privateEcpKeyPath);
    Task<byte[]> CreateMessageSign(byte[] hash, string privateEcpKeyPath);
    Task<bool> VerifySign(byte[] hash, byte[] signature, string publicKeyXml);
}

