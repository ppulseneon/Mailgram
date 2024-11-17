using Mailgram.Server.Models;
using Mailgram.Server.Models.Requests;

namespace Mailgram.Server.Services.Interfaces;

public interface IEmailService
{
    Task SyncAsync(Account account);
    Task<List<Message>> GetAll(Guid userId);
    Task<Message?> ChangeStarred(Account account, uint messageId);
    Task<Message?> DeleteEmail(Account account, uint messageId);
    Task<Message?> SendMessage(Account account, SendMessageRequest request, bool isSwap = false);
    Task<SendMessageRequest> CreateContactMessage(Account account, SendMessageRequest request);
    Task<Message> ReadContactMessage(Account account, Message message);
    Task<string> GetDecryptAttachment(Guid userId, uint messageId, string attachmentName);
}