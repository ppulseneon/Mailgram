using Mailgram.Server.Models;

namespace Mailgram.Server.Repositories.Interfaces;

public interface IContactsRepository
{
    Task<List<Contact>> GetContactsAsync(Guid userId);
}