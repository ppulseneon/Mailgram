namespace Mailgram.Server.Services.Interfaces;

public interface IEncryptService
{
    void EncryptFile(string filepath);
    string DecryptFile(string filepath);
    string CreateMessageSign();
    bool VerifySign();
}