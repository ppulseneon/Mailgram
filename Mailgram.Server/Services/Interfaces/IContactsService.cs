using Mailgram.Server.Models;
using Mailgram.Server.Models.Requests;

namespace Mailgram.Server.Services.Interfaces;

public interface IContactsService
{
    /// <summary>
    /// Получить все контакты
    /// </summary>
    Task<List<Contact>> GetAll(Guid userId);
    
    /// <summary>
    /// Добавить новый контакт
    /// </summary>
    Task<Contact> Add(Account account, ContactRequest request);
    
    /// <summary>
    /// Подтвердить дружбу с контактом
    /// </summary>
    Task Accept(Account account, ContactRequest request);
}

