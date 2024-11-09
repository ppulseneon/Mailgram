using Mailgram.Server.Extensions.Mappers;
using Mailgram.Server.Models.Responses;
using Mailgram.Server.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Mailgram.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController(IAccountService accountService, IEmailReaderService emailReaderService) : ControllerBase
{
    [HttpPost("sync", Name = "SyncEmail")]
    public async Task<ActionResult> Synchronization(Guid id)
    {
        var account = await accountService.Get(id);

        if (account == null)
        {
            return NotFound();
        }
        
        await emailReaderService.SyncAsync(account);
        return Ok();
    }
    
    [HttpGet(Name = "GetEmails")]
    public async Task<ActionResult<List<MessagesResponse>>> Get(Guid id)
    {
        var account = await accountService.Get(id);

        if (account == null)
        {
            return NotFound();
        }
        
        var messages = await emailReaderService.GetAll(id);
        
        return Ok(messages.ToResponse());
    }
    
    // Send Email (synhronize with server)
    
    // Switch folder email
}