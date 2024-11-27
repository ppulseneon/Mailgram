using Mailgram.Server.Enums;
using Mailgram.Server.Models;
using Mailgram.Server.Models.Requests;
using Mailgram.Server.Models.Responses;

namespace Mailgram.Server.Extensions.Mappers;

public static class ContactMapper
{
    public static Contact ToContact(this ContactRequest request)
    {
        return new Contact
        {
            Email = request.Email,
            Status = ExchangeStatus.Sent
        };
    }

    public static ContactResponse ToResponse(this Contact contact)
    {
        return new ContactResponse(contact);
    }
    
    public static ContactsResponse ToResponse(this List<Contact> contacts)
    {
        return new ContactsResponse(contacts);
    }
}