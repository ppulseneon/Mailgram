using Mailgram.Server.Models;
using Mailgram.Server.Models.Requests;
using Mailgram.Server.Models.Responses;

namespace Mailgram.Server.Extensions.Mappers;

public static class AccountMapper
{
    public static Account ToAccount(this AccountRequest request)
    {
        return new Account
        {
            Login = request.Login,
            Password = request.Password,
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