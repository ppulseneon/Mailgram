using Mailgram.Server.Enums;
using Mailgram.Server.Extensions.Mappers;
using Mailgram.Server.Models;
using Mailgram.Server.Models.Requests;
using Mailgram.Server.Repositories.Interfaces;
using Mailgram.Server.Services.Interfaces;
using Mailgram.Server.Tools;
using Newtonsoft.Json;

namespace Mailgram.Server.Services;

public class ContactsService(IContactsRepository contactsRepository, IEmailService emailService): IContactsService
{
    public async Task<List<Contact>> GetAll(Guid userId)
    {
        return await contactsRepository.GetContactsAsync(userId);
    }

    public async Task<Contact> Add(Account account, ContactRequest request)
    {
        var contact = request.ToContact();
        await contactsRepository.SaveContact(account.Id, contact);
        var (rsa, ecp) = await contactsRepository.GenerateContactKeys(account.Id, contact.Email);

        var contactSwap = new ContactSwap
        {
            From = account.Login.AppendDomain(account.Platform),
            SwapStatus = SwapStatus.Request,
            PublicRsa = rsa,
            PublicEcp = ecp
        };

        var message = new SendMessageRequest
        {
            UserId = account.Id,
            To = request.Email,
            Message = JsonConvert.SerializeObject(contactSwap)
        };
        
        await emailService.SendMessage(account, message, isSwap: true);
        
        return contact;
    }
}