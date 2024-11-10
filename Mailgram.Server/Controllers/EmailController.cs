using Mailgram.Server.Extensions.Mappers;
using Mailgram.Server.Models;
using Mailgram.Server.Models.Requests;
using Mailgram.Server.Models.Responses;
using Mailgram.Server.Services.Interface;
using Mailgram.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Mailgram.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController(IAccountService accountService, IEmailService emailService) : ControllerBase
{
    [HttpPost("sync", Name = "SyncEmail")]
    public async Task<ActionResult> Synchronization(Guid id)
    {
        var account = await accountService.Get(id);

        if (account == null)
        {
            return NotFound();
        }

        await emailService.SyncAsync(account);
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

        var messages = await emailService.GetAll(id);

        return Ok(messages.ToResponse());
    }

    [HttpPost("changeStarred", Name = "ChangeStarred")]
    public async Task<ActionResult> ChangeStarred(Guid userId, uint messageId)
    {
        var account = await accountService.Get(userId);

        if (account == null)
        {
            return NotFound("Аккаунт не найден");
        }
        
        var result = await emailService.ChangeStarred(account, messageId);
        
        if (result == null)
        {
            return NotFound("Письмо не найдено");
        }
        
        return Ok(result);
    }

    [HttpDelete(Name = "Delete")]
    public async Task<ActionResult<Message>> DeleteEmail(Guid userId, uint messageId)
    {
        var account = await accountService.Get(userId);

        if (account == null)
        {
            return NotFound("Аккаунт не найден");
        }
        
        var result = await emailService.DeleteEmail(account, messageId);
        
        if (result == null)
        {
            return NotFound("Письмо не найдено");
        }
        
        return Ok(result);
    }
    
    [HttpPost(Name = "Send")]
    public async Task<ActionResult<Message>> SendEmail(SendMessageRequest request)
    {
        var account = await accountService.Get(request.UserId);
        
        if (account == null)
        {
            return NotFound("Аккаунт не найден");
        }
        
        var result = await emailService.SendMessage(account, request);
        
        if (result == null)
        {
            return BadRequest("Проблемы с отправкой письма");
        }
        
        return Ok(result);
    }
    
    [HttpGet("attachment")]
    public async Task<ActionResult<Message>> GetDecryptAttachment(Guid userId, uint messageId, string attachmentName)
    {
        var account = await accountService.Get(userId);
        
        if (account == null)
        {
            return NotFound("Аккаунт не найден");
        }

        var result = await emailService.GetDecryptAttachment(userId, messageId, attachmentName);
        
        if (string.IsNullOrEmpty(result))
        {
            return BadRequest("Проблемы с получением вложения");
        }
        
        return Ok(result);
    }
}