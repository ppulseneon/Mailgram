namespace Mailgram.Server.Services.Interfaces;

public interface IEncryptService
{
    Task EncryptFile(string filepath);
    string DecryptFile(string filepath);
    string CreateMessageSign();
    bool VerifySign();
}