using System.Security.Cryptography;
using Mailgram.Server.Services.Interfaces;

namespace Mailgram.Server.Services;

public class EncryptService: IEncryptService
{
    public void EncryptFile(string filepath)
    {
        throw new NotImplementedException();
    }

    public string DecryptFile(string filepath)
    {
        throw new NotImplementedException();
    }

    public string CreateMessageSign()
    {
        throw new NotImplementedException();
    }

    public bool VerifySign()
    {
        throw new NotImplementedException();
    }
}