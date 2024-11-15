using Mailgram.Server.Extensions.Mappers;
using Mailgram.Server.Models.Responses;
using Mailgram.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Mailgram.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController(IContactsService contactsService): ControllerBase
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
}