namespace Mailgram.Server.Services.Interfaces;

public interface IEncryptService
{
    Task<string> GenerateRsa(string filepath, string privateKeyName);
    Task EncryptMessage();
    Task EncryptFile(string filepath);
    string DecryptFile(string filepath);
    string CreateMessageSign();
    bool VerifySign();
}