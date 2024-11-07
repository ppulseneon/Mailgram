using System.Text;
using Mailgram.Server.Utility;

namespace Mailgram.Server.Models;

public class Account
{
    public Guid Id { get; set; }
    public required string Login { get; init; }
    public string EncryptedPassword { get; init; }
    public required byte[] Salt { get; init; }
    public required string Platform { get; init; }
    public ConnectionCredentials SmtpCredentials { get; set; }
    public ConnectionCredentials ImapCredentials { get; set; }

    public string GetPassword()
    {
        var security = new PasswordSecurity();
        
        var decryptedHashedPassword = security.DecryptPassword(EncryptedPassword);
        var decryptedPassword = Encoding.UTF8.GetString(decryptedHashedPassword);
        
        return decryptedPassword;
    }
}
