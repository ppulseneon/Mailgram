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
        
        await emailReaderService.LoadEmailsAsync(account);
        return Ok();
    }
    
    [HttpGet(Name = "GetEmail")]
    public async Task<ActionResult<List<MessageResponse>>> Get(Guid id)
    {
        var messages = await 
        
        return Ok(new List<MessageResponse>());
    }
    
    // Send Email (synhronize with server)
    
    // Switch folder email
}