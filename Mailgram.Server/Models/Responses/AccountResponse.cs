namespace Mailgram.Server.Models.Responses;

public class AccountResponse(Account account)
{
    public Guid Id { get; set; } = account.Id;
    public string Login { get; set; } = account.Login;
    public string Platform { get; set; } = account.Platform;
}