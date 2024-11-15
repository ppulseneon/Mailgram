using Mailgram.Server.Models;
using Mailgram.Server.Repositories.Interfaces;
using Mailgram.Server.Services.Interfaces;

namespace Mailgram.Server.Services;

public class ContactsService(IContactsRepository contactsRepository): IContactsService
{
    public async Task<List<Contact>> GetAll(Guid userId)
    {
        return await contactsRepository.GetContactsAsync(userId);
    }
}