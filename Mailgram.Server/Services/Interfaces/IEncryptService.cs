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
    Task<byte[]> DecryptKey(byte[] desKey, string privateRsaKeyPath);
    Task<byte[]> DecryptFile(byte[] data, byte[] desKey, byte[] iv);
    string CreateMessageSign();
    bool VerifySign();
}