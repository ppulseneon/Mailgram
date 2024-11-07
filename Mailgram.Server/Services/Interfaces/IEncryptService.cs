namespace Mailgram.Server.Services.Interfaces;

public interface IEncryptService
{
    Task EncryptMessage();
    Task EncryptFile(string filepath);
    string DecryptFile(string filepath);
    string CreateMessageSign();
    bool VerifySign();
}