namespace Mailgram.Server.Services.Interfaces;

public interface IEncryptService
{
    Task<string> GenerateRsa(string filepath, string privateKeyName);
    Task<(byte[], byte[])> GenerateTripleDes();
    Task<string> EncryptMessage(string message, byte[] desKey, byte[] iv);
    Task<string> EncryptFile(IFormFile file, string subTempFolderName, byte[] desKey, byte[] iv);
    Task<string> EncryptFile(byte[] data, string subTempFolderName, string filename, byte[] desKey, byte[] iv);
    Task<string> EncryptKey(byte[] desKey, string subTempFolderName, string publicRsaKeyPath);
    string DecryptFile(string filepath);
    string CreateMessageSign();
    bool VerifySign();
}