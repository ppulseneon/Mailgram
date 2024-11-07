using Mailgram.Server.Models;
using Mailgram.Server.Models.Requests;
using Mailgram.Server.Models.Responses;

namespace Mailgram.Server.Services.Interface;

public interface IAccountService
{
    public Task<Account?> Add(AccountRequest request);

    public Task<Account?> Get(string email);
    
    public Task<Account?> Get(Guid id);
    
    public Task<List<Account>> GetAll();
}