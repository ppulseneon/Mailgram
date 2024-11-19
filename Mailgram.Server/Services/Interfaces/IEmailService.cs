using Mailgram.Server.Models;
using Mailgram.Server.Models.Requests;
using MimeKit;

namespace Mailgram.Server.Services.Interfaces;

public interface IEmailService
{
    Task SyncAsync(Account account);
    Task<List<Message>> GetAll(Guid userId);
    Task<Message?> ChangeStarred(Account account, int messageId);
    Task<Message?> DeleteEmail(Account account, int messageId);
    Task<Message?> SendMessage(Account account, SendMessageRequest request, bool isSwap = false);
    Task<SendMessageRequest> CreateContactMessage(Account account, SendMessageRequest request);
    Task<Message> SendMessageRequestToMessage(Account account, SendMessageRequest request);
    Task<string> GetDecryptAttachment(Guid userId, int messageId, string attachmentName);
}