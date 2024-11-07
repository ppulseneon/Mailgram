using Mailgram.Server.Models;
using Mailgram.Server.Models.Requests;
using Mailgram.Server.Models.Responses;
using Mailgram.Server.Utility;

namespace Mailgram.Server.Extensions.Mappers;

public static class AccountMapper
{
    public static Account ToAccount(this AccountRequest request)
    {
        var security = new PasswordSecurity();
        var salt = security.GenerateSalt();
        var hashedPassword = security.HashPassword(request.Password, salt);
        
        return new Account
        {
            Login = request.Login,
            EncryptedPassword = security.EncryptPassword(hashedPassword),
            Salt = salt,
            SmtpCredentials = request.SmtpCredentials,
            ImapCredentials = request.ImapCredentials,
            Platform = request.Platform,
        };
    }

    public static AccountResponse ToResponse(this Account account)
    {
        return new AccountResponse(account);
    }
    
    public static AccountsResponse ToResponse(this List<Account> accounts)
    {
        return new AccountsResponse(accounts);
    }
}