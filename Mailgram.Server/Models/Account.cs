namespace Mailgram.Server.Models;

public class Account
{
    public Guid Id { get; set; }
    public required string Login { get; init; }
    public string Password { get; init; }
    public required string Platform { get; init; }
    public ConnectionCredentials SmtpCredentials { get; set; }
    public ConnectionCredentials ImapCredentials { get; set; }
}
