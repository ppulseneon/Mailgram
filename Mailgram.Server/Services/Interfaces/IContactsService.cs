using Mailgram.Server.Models;

namespace Mailgram.Server.Services.Interfaces;

public interface IContactsService
{
    Task<List<Contact>> GetAll(Guid userId);
}