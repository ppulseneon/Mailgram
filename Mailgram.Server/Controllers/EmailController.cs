using Mailgram.Server.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Mailgram.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController(IAccountService accountService, IEmailReaderService emailReaderService) : ControllerBase
{
    [HttpGet(Name = "GetEmail")]
    public async Task<ActionResult> Get(Guid id) // IEnumerable<WeatherForecast>
    {
        var account = await accountService.Get(id);

        if (account == null)
        {
            return NotFound();
        }
        
        await emailReaderService.LoadEmailsAsync(account);
        return Ok();
    }
}