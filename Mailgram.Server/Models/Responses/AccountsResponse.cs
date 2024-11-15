namespace Mailgram.Server.Models.Responses;

public class AccountsResponse(List<Account> accounts)
{
    public IList<AccountResponse> Accounts { get; set; } = accounts.Select(x => new AccountResponse(x)).ToList();
    public int Count { get; set; } = accounts.Count;
}