using Mailgram.Server.Enums;

namespace Mailgram.Server.Models.Responses;

public class ContactResponse(Contact contact)
{
    public string Email { get; set; } = contact.Email;
    public ExchangeStatus Status { get; set; } = contact.Status;
}