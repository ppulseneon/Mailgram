using Mailgram.Server.Models;
using Mailgram.Server.Models.Requests;

namespace Mailgram.Server.Services.Interfaces;

public interface IContactsService
{
    Task<List<Contact>> GetAll(Guid userId);
    Task<Contact> Add(Account account, ContactRequest request);
}