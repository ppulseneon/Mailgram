using Mailgram.Server.Extensions.Mappers;
using Mailgram.Server.Models;
using Mailgram.Server.Models.Requests;
using Mailgram.Server.Repositories.Interfaces;
using Mailgram.Server.Services.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Mailgram.Server.Services;

public class AccountService(IAccountsRepository accountsRepository) : IAccountService
{
    public async Task<Account?> Add(AccountRequest request)
    {
        var canLogin = await TryLogin(request);

        if (!canLogin)
            return null;
        
        var account = request.ToAccount();
        
        await accountsRepository.CreateAccountAsync(account);
        
        return account;
    }

    public async Task<Account?> Get(string email)
    {
        return await accountsRepository.GetAccountAsync(email);
    }
    
    public async Task<Account?> Get(Guid id)
    {
        return await accountsRepository.GetAccountAsync(id);
    }

    public async Task<List<Account>> GetAll()
    {
        return await accountsRepository.GetAccountsAsync();
    }

    private async Task<bool> TryLogin(AccountRequest request)
    {
        try
        {
            using var client = new SmtpClient();
            client.Timeout = 5000; // 5 sec
            
            var host = request.SmtpCredentials.Hostname;
            var port = request.SmtpCredentials.Port;
            var username = request.Login;
            var password = request.Password;

            await client.ConnectAsync(host, port, SecureSocketOptions.SslOnConnect);
            
            await client.AuthenticateAsync(username, password);

            Console.WriteLine("Connection successful!");

            await client.DisconnectAsync(true);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Authentication failed: {ex.Message}");
            return false;
        }
    }
}