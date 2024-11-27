namespace Mailgram.Server.Models.Requests;

public class AccountRequest
{
    public required string Login { get; set; }
    public required string Password { get; init; }
    public required string Platform { get; init; }
    public ConnectionCredentials SmtpCredentials { get; set; }
    public ConnectionCredentials ImapCredentials { get; set; }
}