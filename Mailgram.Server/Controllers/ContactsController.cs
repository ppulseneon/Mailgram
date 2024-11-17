using Mailgram.Server.Extensions.Mappers;
using Mailgram.Server.Models.Requests;
using Mailgram.Server.Models.Responses;
using Mailgram.Server.Services.Interface;
using Mailgram.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Mailgram.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController(IContactsService contactsService, IAccountService accountService) : ControllerBase
{
    [HttpGet(Name = "GetContacts")]
    public async Task<ActionResult<ContactsResponse>> Get(Guid userId)
    {
        var contacts = await contactsService.GetAll(userId);
        return Ok(contacts.ToResponse());
    }

    [HttpPost(Name = "CreateContact")]
    public async Task<ActionResult<ContactResponse>> Create(Guid userId, ContactRequest request)
    {
        var account = await accountService.Get(userId);

        if (account is null)
        {
            return NotFound("Аккаунт не найден");
        }

        var contact = await contactsService.Add(account, request);
        return Ok(contact.ToResponse());
    }

    [HttpPost("accept", Name = "AcceptContact")]
    public async Task<ActionResult<ContactResponse>> Accept(Guid userId, string email)
    {
        var account = await accountService.Get(userId);
        
        if (account is null)
        {
            return NotFound("Аккаунт не найден");
        }
        
        var contacts = await contactsService.GetAll(userId);

        var findContact = contacts.FirstOrDefault(x => x.Email == email);

        if (findContact is null)
        {
            return NotFound("Контакт не найден");
        }

        var request = new ContactRequest
        {
            Email = email
        };
        
        await contactsService.Accept(account!, request);
        
        return Ok(findContact.ToResponse());
    }
}