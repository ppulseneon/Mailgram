namespace Mailgram.Server.Models.Responses;

public class AccountsResponse
{
    public AccountsResponse(List<Account> accounts)
    {   
        Count = accounts.Count;
        Accounts = accounts.Select(x => new AccountResponse(x)).ToList();
    }
    
    public IList<AccountResponse> Accounts { get; set; }
    public int Count { get; set; }
}