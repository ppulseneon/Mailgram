using Mailgram.Server.Models;
using Mailgram.Server.Models.Requests;
using Mailgram.Server.Models.Responses;

namespace Mailgram.Server.Services.Interface;

public interface IAccountService
{
    /// <summary>
    /// Добавить аккаунт
    /// </summary>
    public Task<Account?> Add(AccountRequest request);

    /// <summary>
    /// Получить аккаунт по email
    /// </summary>
    public Task<Account?> Get(string email);
    
    /// <summary>
    /// Получить аккаунт по Id
    /// </summary>
    public Task<Account?> Get(Guid id);
    
    /// <summary>
    /// Получить все аккаунты
    /// </summary>
    public Task<List<Account>> GetAll();
}

