using Mailgram.Server.Extensions.Mappers;
using Mailgram.Server.Models.Requests;
using Mailgram.Server.Models.Responses;
using Mailgram.Server.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Mailgram.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController(IAccountService accountService) : ControllerBase
{
    [HttpGet(Name = "GetAccounts")]
    public async Task<ActionResult<IEnumerable<AccountResponse>>> Get()
    {
        var accounts = await accountService.GetAll();
        return Ok(accounts.ToResponse());
    }

    [HttpGet("{email}", Name = "GetAccount")]
    public async Task<ActionResult<AccountResponse>> Get(string email)
    {
        var account = await accountService.Get(email);

        if (account == null)
        {
            return NotFound();
        }

        return Ok(account.ToResponse());
    }

    [HttpPost(Name = "AddAccount")]
    public async Task<ActionResult<AccountsResponse>> Add(AccountRequest request)
    {
        // Преобразуем почту в логин пользователя
        request.Login = request.Login.Split('@')[0];
        
        var result = await accountService.Add(request);
        return result != null ? Ok(result.ToResponse()) : BadRequest("Неверный логин или пароль");
    }
}