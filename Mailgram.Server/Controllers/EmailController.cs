using Mailgram.Server.Extensions.Mappers;
using Mailgram.Server.Models;
using Mailgram.Server.Models.Requests;
using Mailgram.Server.Models.Responses;
using Mailgram.Server.Repositories.Interfaces;
using Mailgram.Server.Services.Interface;
using Mailgram.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Mailgram.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController(IAccountService accountService, IEmailService emailService, IMessagesRepository messagesRepository) : ControllerBase
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
    public async Task<ActionResult> ChangeStarred(Guid userId, int messageId)
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
    public async Task<ActionResult<Message>> DeleteEmail(Guid userId, int messageId)
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
        var requestCopy = request;
        
        if (account == null)
        {
            return NotFound("Аккаунт не найден");
        }
        
        if (request.IsEncrypt || request.IsSign)
        {
            // Шифруем сообщение
            request = await emailService.CreateContactMessage(account, request);
        }
        
        // Отправляем сообщение
        var result = await emailService.SendMessage(account, request);
        
        if (request.IsEncrypt || request.IsSign)
        {
            // Форматируем оригинальное сообщение
            result = await emailService.SendMessageRequestToMessage(account, requestCopy);
        }

        // Список файлов прикрепленных к сообщению
        var attachmentsMessageList = new List<string>();
        
        // Сохраняем в сообщение отправленные файлы
        if (requestCopy.Attachments != null)
        {
            attachmentsMessageList.AddRange(requestCopy.Attachments.Select(attachment => attachment.FileName));
        }

        result!.AttachmentFiles = attachmentsMessageList;
        
        // Сохраняем сообщение
        await messagesRepository.SaveMessage(account.Id, result);  
        
        // Сохраняем вложения
        if (requestCopy.Attachments != null)
        {
            foreach (var attachment in requestCopy.Attachments)
            {
                try
                {
                    var fileName = Path.GetFileName(attachment.FileName);

                    using var memoryStream = new MemoryStream();
                    await attachment.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();

                    await messagesRepository.SaveAttachment(account.Id, result!.Id, fileName, content);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving attachment sent file: {ex}");
                }
            }
        }
        
        if (result == null)
        {
            return BadRequest("Проблемы с отправкой письма");
        }
        
        return Ok(result);
    }
    
    [HttpGet("attachment")]
    public async Task<ActionResult<Message>> GetDecryptAttachment(Guid userId, int messageId, string attachmentName)
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