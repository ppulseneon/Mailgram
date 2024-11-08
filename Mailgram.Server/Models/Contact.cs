using Mailgram.Server.Enums;

namespace Mailgram.Server.Models;

public class Contact
{
    public string Email { get; set; }
    public ExchangeStatus Status { get; set; }
}