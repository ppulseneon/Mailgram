using Mailgram.Server.Enums;

namespace Mailgram.Server.Models.Responses;

public class ContactResponse
{
    public string Email { get; set; }
    public ExchangeStatus Status { get; set; }
}