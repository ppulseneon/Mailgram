using Mailgram.Server.Extensions.Mappers;
using Mailgram.Server.Models.Requests;
using Mailgram.Server.Models.Responses;
using Mailgram.Server.Services.Interface;
using Mailgram.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Mailgram.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController(IContactsService contactsService, IAccountService accountService): ControllerBase
{
    // todo: contacts reqeuest
    // todo: send contact add
    // todo: access contact
    // todo: deny contact
    
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
            return NotFound();
        }
        
        var contact = await contactsService.Add(account, request);
        return Ok(contact.ToResponse());
    }
}