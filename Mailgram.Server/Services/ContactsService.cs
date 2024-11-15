using Mailgram.Server.Extensions.Mappers;
using Mailgram.Server.Models;
using Mailgram.Server.Models.Requests;
using Mailgram.Server.Repositories.Interfaces;
using Mailgram.Server.Services.Interfaces;

namespace Mailgram.Server.Services;

public class ContactsService(IContactsRepository contactsRepository): IContactsService
{
    public async Task<List<Contact>> GetAll(Guid userId)
    {
        return await contactsRepository.GetContactsAsync(userId);
    }

    public async Task<Contact> Add(Guid userId, ContactRequest request)
    {
        var contact = request.ToContact();
        await contactsRepository.SaveContact(userId, contact);
        
        return contact;
    }
}