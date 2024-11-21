using Mailgram.Server.Models;

namespace Mailgram.Server.Repositories.Interfaces;

public interface IAccountsRepository
{ 
    Task CreateAccountAsync(Account account);
    Task<List<Account>> GetAccountsAsync();
    Task<Account?> GetAccountAsync(string email);
    Task<Account?> GetAccountAsync(Guid id);
}